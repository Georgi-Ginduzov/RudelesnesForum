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

        public async Task<IActionResult> Details(
            int id,
            int replyPage = 1,
            string sortReplies = "oldest",
            int replyPageSize = 10)
        {
            var postDetailsModel = new PostDetailsViewModel
            {
                IsLoggedIn = true,
                CurrentUserId = "user-123",
                CanReply = true,
                CanEdit = true,
                CanDelete = false,
                CanModerate = false,

                Post = new PostDetail
                {
                    Id = 5,
                    Title = "How to use ML.NET for sentiment analysis?",
                    Content = "I'm trying to classify comments as rude or polite using ML.NET. Anyone done this?",
                    AuthorName = "MLBeginner",
                    AuthorId = "user-123",
                    AuthorAvatar = "/images/avatars/mlbeginner.png",
                    AuthorRole = "Member",
                    CreatedAt = DateTime.UtcNow.AddDays(-2),
                    UpdatedAt = DateTime.UtcNow.AddDays(-1),
                    ViewCount = 213,
                    ReplyCount = 3,
                    CategoryName = "Machine Learning",
                    CategoryId = 2,
                    CategoryColor = "info",
                    IsPinned = false,
                    IsLocked = false,
                    Tags = new List<string> { "ml.net", "classification", "help" },
                    LikeCount = 12,
                    IsLikedByCurrentUser = true
                },

                Replies = new List<PostReply>
                {
                    new PostReply
                    {
                        Id = 1,
                        Content = "Yes! I trained a custom model using a small dataset. Use `TextFeaturizingEstimator`.",
                        AuthorName = "DataNerd",
                        AuthorId = "user-456",
                        AuthorAvatar = "/images/avatars/data_nerd.png",
                        AuthorRole = "Member",
                        CreatedAt = DateTime.UtcNow.AddDays(-1).AddHours(-4),
                        LikeCount = 4,
                        IsLikedByCurrentUser = false,
                        IsAuthor = false,
                        ParentReplyId = null
                    },
                    new PostReply
                    {
                        Id = 2,
                        Content = "Make sure your training data is clean. That helped me a lot.",
                        AuthorName = "MLGuru",
                        AuthorId = "user-789",
                        AuthorAvatar = "/images/avatars/mlguru.png",
                        AuthorRole = "Moderator",
                        CreatedAt = DateTime.UtcNow.AddHours(-22),
                        LikeCount = 6,
                        IsLikedByCurrentUser = true,
                        IsAuthor = false,
                        ParentReplyId = null
                    },
                    new PostReply
                    {
                        Id = 3,
                        Content = "@MLGuru thanks! Did you use a pre-trained model or start from scratch?",
                        AuthorName = "MLBeginner",
                        AuthorId = "user-123",
                        AuthorAvatar = "/images/avatars/mlbeginner.png",
                        AuthorRole = "Member",
                        CreatedAt = DateTime.UtcNow.AddHours(-12),
                        LikeCount = 2,
                        IsLikedByCurrentUser = false,
                        IsAuthor = true,
                        ParentReplyId = 2,
                        ParentAuthorName = "MLGuru"
                    }
                },
                ReplyPagination = new PaginationInfo
                {
                    CurrentPage = 1,
                    PageSize = 10,
                    TotalItems = 3,
                    TotalPages = 1
                },
                RelatedPosts = new List<PostDetail>
                {
                    new PostDetail
                    {
                        Id = 6,
                        Title = "Using ML.NET with ASP.NET Core",
                        AuthorName = "DevTom",
                        AuthorId = "user-901",
                        AuthorAvatar = "/images/avatars/devtom.png",
                        CreatedAt = DateTime.UtcNow.AddDays(-3),
                        ViewCount = 94,
                        ReplyCount = 2,
                        CategoryName = "Machine Learning",
                        CategoryId = 2,
                        CategoryColor = "info",
                        IsPinned = false,
                        IsLocked = false,
                        Tags = new List<string> { "ml.net", "integration" },
                        LikeCount = 3,
                        IsLikedByCurrentUser = false
                    }
                }
            };
            return View("Details", postDetailsModel);
        }

        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreatePostViewModel post)
        {
            if (!ModelState.IsValid)
                return View(post);

            var user = await _userManager.GetUserAsync(User);
            var postId = await _postService.CreateAsync(user.Id, post.Title, post.Content);
            return RedirectToAction(nameof(Details), postId);
        }
    }
}
