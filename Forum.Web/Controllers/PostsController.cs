using Forum.Web.Data.Entities;
using Forum.Web.Models;
using Forum.Web.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Forum.Web.Controllers
{
    [Authorize]
    public class PostsController : Controller
    {
        private readonly IPostService _postService;
        private readonly UserManager<ApplicationUser> _userManager;

        public PostsController(IPostService postService, UserManager<ApplicationUser> userManager)
        {
            _postService = postService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(string search = "", int page = 1, int pageSize = 20)
        {
            var posts = await _postService.GetAllPostsAsync(search, (page - 1) *  pageSize, pageSize);
            var totalPostsCount = await _postService.GetPostsCountAsync();
            var model = new PostsOverviewViewModel
            { 
                Posts = posts.Select(x => new PostSummary
                {
                    PostId = x.PostId,
                    Title = x.Title,
                    Content = x.Content,
                    UserId = x.UserId,
                    AuthorName = x.User.UserName,
                    AuthorEmail = x.User.Email,
                    CreatedAt = x.CreatedAt,
                    LastReplyAt = x.Replies?.MaxBy(x => x.CreatedAt)?.CreatedAt,
                    LastReplyBy = x.Replies?.MaxBy(x => x.CreatedAt)?.User?.UserName ?? string.Empty,
                    ReplyCount = x.Replies?.Count ?? 0
                }).ToList(),
                TotalPosts = totalPostsCount,
                SearchQuery = search
            };
            return View(model);
        }

        public async Task<IActionResult> Details(int id)
        {
            var post = await _postService.GetPostByIdAsync(id);
            var user = await _userManager.GetUserAsync(User);
            var isModerator = await _userManager.IsInRoleAsync(user, "Moderator");
            var isAdmin = await _userManager.IsInRoleAsync(user, "Admin");
            var postDetailsModel = new PostDetailsViewModel()
            {
                Post = new PostDetail
                {
                    PostId = id,
                    Title = post.Title,
                    Content = post.Content,
                    UserId = post.UserId,
                    AuthorName = post.User?.UserName ?? string.Empty,
                    AuthorEmail = post.User?.Email ?? string.Empty,
                    CreatedAt = post.CreatedAt,
                    LastUpdated = post.LastUpdated,
                    ReplyCount = post.Replies.Count(x => !x.IsFlagged && x.IsReviewed)
                },
                Replies = post.Replies.Where(x => user?.Id == x.UserId || 
                                                  isAdmin || isModerator || 
                                                  !x.IsFlagged && x.IsReviewed)
                .Select(x => new PostReply 
                {
                    ReplyId = x.Id,
                    Content = x.Content,
                    UserId = x.UserId,
                    AuthorName = x.User?.UserName ?? string.Empty,
                    AuthorEmail = x.User?.Email ?? string.Empty ,
                    CreatedAt = x.CreatedAt,
                    LastUpdated = x.UpdatedAt == null ? x.CreatedAt : (DateTime)x.UpdatedAt,
                    IsReviewed = x.IsReviewed,
                    IsFlagged = x.IsFlagged,
                }).ToList(),
                CanEdit = user!.Id == post.UserId,
                CanDelete = user.Id == post.UserId,
                CurrentUserId = user.Id
            };
            if (TempData.ContainsKey("ReplySubmitted"))
            {
                postDetailsModel.ShowSubmissionNotice = true;
                postDetailsModel.IsSubmissionFlagged = TempData.ContainsKey("ReplyFlagged") && (bool)TempData["ReplyFlagged"];
            }
            TempData.Clear();
            return View("Details", postDetailsModel);
        }

        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(CreatePostViewModel post)
        {
            if (!ModelState.IsValid)
                return View(post);

            var user = await _userManager.GetUserAsync(User);
            var postId = await _postService.CreateAsync(user.Id, post.Title, post.Content);
            return RedirectToAction(nameof(Details), new { Id = postId });
        }

        [HttpPost("Posts/{postId}/Reply")]
        public async Task<IActionResult> PostReply(int postId, string content)
        {
            var user = await _userManager.GetUserAsync(User);
            var (_, isFlagged) = await _postService.AddPostReplyAsync(user.Id, postId, content);
            TempData["ReplySubmitted"] = true;
            TempData["ReplyFlagged"] = isFlagged;
            return RedirectToAction("Details", new { id = postId });
        }

        [HttpPost("Posts/Delete/{id}")]
        public async Task<IActionResult> DeletePost(int id)
        {
            await _postService.DeletePostAsync(id);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteReply(int id)
        {
            var postId = await _postService.DeleteReplyAsync(id);
            return RedirectToAction("Details", new { id = postId });
        }

    }
}
