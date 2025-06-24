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

        public async Task<IActionResult> Index(
                    string search = "",
                    int? categoryId = null,
                    string sortBy = "recent",
                    string timeFilter = "all",
                    bool showPinnedOnly = false,
                    int page = 1,
                    int pageSize = 20)
        {
            var model = new PostsOverviewViewModel
            {
                TotalPosts = 128,
                SearchQuery = "ml.net",
                CurrentSort = "popular",

                Filter = new PostsFilter
                {
                    CategoryId = 2,
                    SortBy = "popular",
                    TimeFilter = "week",
                    ShowPinnedOnly = false
                },

                Pagination = new PaginationInfo
                {
                    CurrentPage = 1,
                    PageSize = 10,
                    TotalItems = 28,
                    TotalPages = 3
                },

                Categories = new List<ForumCategory>
                {
                    new ForumCategory
                    {
                        Id = 1,
                        Name = "General",
                        Description = "Talk about anything!",
                        PostCount = 45,
                        IconClass = "fas fa-comments"
                    },
                    new ForumCategory
                    {
                        Id = 2,
                        Name = "Machine Learning",
                        Description = "All things ML, AI, and data science.",
                        PostCount = 31,
                        IconClass = "fas fa-brain"
                    },
                    new ForumCategory
                    {
                        Id = 3,
                        Name = "Announcements",
                        Description = "Official updates and rules.",
                        PostCount = 12,
                        IconClass = "fas fa-bullhorn"
                    }
                },

                Posts = new List<PostSummary>
                {
                    new PostSummary
                    {
                        Id = 1,
                        Title = "Getting started with ML.NET",
                        Content = "Can anyone recommend a good ML.NET tutorial?",
                        AuthorName = "DataDabbler",
                        AuthorAvatar = "/images/avatars/user1.png",
                        CreatedAt = DateTime.UtcNow.AddDays(-2),
                        LastReplyAt = DateTime.UtcNow.AddHours(-5),
                        LastReplyBy = "MLGuru",
                        ReplyCount = 7,
                        ViewCount = 145,
                        CategoryName = "Machine Learning",
                        CategoryColor = "info",
                        IsPinned = false,
                        IsLocked = false,
                        HasUnreadReplies = true,
                        Tags = new List<string> { "ml.net", "beginners", "help" }
                    },
                    new PostSummary
                    {
                        Id = 2,
                        Title = "Forum Rules & Guidelines",
                        Content = "Please read before posting.",
                        AuthorName = "AdminUser",
                        AuthorAvatar = "/images/avatars/admin.png",
                        CreatedAt = DateTime.UtcNow.AddMonths(-1),
                        LastReplyAt = null,
                        LastReplyBy = null,
                        ReplyCount = 0,
                        ViewCount = 300,
                        CategoryName = "Announcements",
                        CategoryColor = "warning",
                        IsPinned = true,
                        IsLocked = true,
                        HasUnreadReplies = false,
                        Tags = new List<string> { "rules", "pinned" }
                    },
                    new PostSummary
                    {
                        Id = 3,
                        Title = "Showcase your ML.NET projects!",
                        Content = "Let's share what we've built with ML.NET.",
                        AuthorName = "CoolDev42",
                        AuthorAvatar = "/images/avatars/user2.png",
                        CreatedAt = DateTime.UtcNow.AddDays(-5),
                        LastReplyAt = DateTime.UtcNow.AddHours(-1),
                        LastReplyBy = "DataFan",
                        ReplyCount = 9,
                        ViewCount = 112,
                        CategoryName = "Machine Learning",
                        CategoryColor = "info",
                        IsPinned = false,
                        IsLocked = false,
                        HasUnreadReplies = false,
                        Tags = new List<string> { "showcase", "projects", "ml.net" }
                    }
                }
            };
            return View(model);
        }

        public async Task<IActionResult> Details(int id)
        {
            var post = await _postService.GetPostByIdAsync(id);
            var user = await _userManager.GetUserAsync(User);
            var postDetailsModel = new PostDetailsViewModel()
            {
                Post = new PostDetail
                {
                    PostId = id,
                    Title = post.Title,
                    Content = post.Content,
                    UserId = post.UserId,
                    AuthorName = post.User.UserName!,
                    AuthorEmail = post.User.Email!,
                    CreatedAt = post.CreatedAt,
                    LastUpdated = post.LastUpdated,
                    ReplyCount = post.Replies.Count
                },
                Replies = post.Replies.Select(x => new PostReply 
                {
                    ReplyId = x.Id,
                    Content = x.Content,
                    UserId = x.UserId,
                    AuthorName = x.User.UserName!,
                    AuthorEmail = x.User.Email!,
                    CreatedAt = x.CreatedAt,
                    LastUpdated = (DateTime)x.UpdatedAt!
                }).ToList(),
                CanEdit = user!.Id == post.UserId,
                CanDelete = user.Id == post.UserId,
                CurrentUserId = user.Id
            };
            return View("Details", postDetailsModel);
        }

        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost("Posts/{postId}/Reply")]
        public async Task<IActionResult> PostReply(int postId, string content)
        {
            var user = await _userManager.GetUserAsync(User);
            await _postService.AddPostReplyAsync(user.Id, postId, content);
            return RedirectToAction("Details", new { id = postId });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteReply(int id)
        {
            var postId = await _postService.DeleteReplyAsync(id);
            return RedirectToAction("Details", new { id = postId });
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreatePostViewModel post)
        {
            if (!ModelState.IsValid)
                return View(post);

            var user = await _userManager.GetUserAsync(User);
            var postId = await _postService.CreateAsync(user.Id, post.Title, post.Content);
            return RedirectToAction(nameof(Details), new { Id = postId });
        }
    }
}
