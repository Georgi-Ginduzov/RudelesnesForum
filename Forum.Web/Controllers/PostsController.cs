using Forum.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace Forum.Web.Controllers
{
    public class PostsController : Controller
    {
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
    }
}
