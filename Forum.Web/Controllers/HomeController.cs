using Forum.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace Forum.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            if (User.IsInRole("Admin"))
            {
                var model = new AdminDashboardViewModel
                {
                    UsersCount = 5,
                    ModeratorsCount = 3,
                    AdminsCount = 14,
                    TotalUsersCount = 10
                };
                return View("Admin", model);
            }
            else if (User.IsInRole("Moderator"))
            {
                var model = new ModeratorDashboardViewModel
                {
                    PendingReports = 3,
                    ActiveWarnings = 4,
                    FlaggedPosts = 5,
                    ResolvedToday = 6,
                };
                return View("Moderator", model);
            }
            else if (User.IsInRole("User"))
            {
                var model = new UserHomeViewModel
                {
                    TotalPosts = 128,
                    TotalUsers = 57,
                    OnlineUsers = 6,
                    TodaysPosts = 9,
                    LatestDiscussions = new List<LatestDiscussion>
                    {
                        new LatestDiscussion
                        {
                            Id = 1,
                            Title = "Welcome to the Forum!",
                            AuthorName = "AdminUser",
                            CreatedAt = DateTime.UtcNow.AddDays(-3),
                            ReplyCount = 5,
                            LastReplyBy = "HelpfulGuy42",
                            LastReplyAt = DateTime.UtcNow.AddHours(-2),
                            Category = "Announcements",
                            IsPinned = true,
                            IsLocked = false
                        },
                        new LatestDiscussion
                        {
                            Id = 2,
                            Title = "Best .NET Practices in 2025?",
                            AuthorName = "CodeMaster",
                            CreatedAt = DateTime.UtcNow.AddDays(-1),
                            ReplyCount = 12,
                            LastReplyBy = "DevDiva",
                            LastReplyAt = DateTime.UtcNow.AddMinutes(-30),
                            Category = "Development",
                            IsPinned = false,
                            IsLocked = false
                        },
                        new LatestDiscussion
                        {
                            Id = 3,
                            Title = "Any good ML.NET tutorials?",
                            AuthorName = "MLNoob",
                            CreatedAt = DateTime.UtcNow.AddDays(-2),
                            ReplyCount = 3,
                            LastReplyBy = "DataSensei",
                            LastReplyAt = DateTime.UtcNow.AddHours(-6),
                            Category = "Machine Learning",
                            IsPinned = false,
                            IsLocked = false
                        },
                        new LatestDiscussion
                        {
                            Id = 4,
                            Title = "Forum Rules & Guidelines",
                            AuthorName = "Moderator1",
                            CreatedAt = DateTime.UtcNow.AddMonths(-1),
                            ReplyCount = 0,
                            LastReplyBy = null,
                            LastReplyAt = null,
                            Category = "Announcements",
                            IsPinned = true,
                            IsLocked = true
                        }
                    }
                };
                return View("User", model);
            }
            var guestModel = new GuestHomeViewModel
            {
                TotalPosts = 134,
                TotalMembers = 63,
                OnlineUsers = 7,
                RecentDiscussions = new List<PublicDiscussion>
                {
                    new PublicDiscussion
                    {
                        Id = 1,
                        Title = "Introduce Yourself!",
                        AuthorName = "FirstUser",
                        CreatedAt = DateTime.UtcNow.AddDays(-5),
                        ReplyCount = 8,
                        Category = "General",
                        IsPinned = true
                    },
                    new PublicDiscussion
                    {
                        Id = 2,
                        Title = "Forum Suggestions",
                        AuthorName = "FeedbackFan",
                        CreatedAt = DateTime.UtcNow.AddDays(-2),
                        ReplyCount = 3,
                        Category = "Feedback",
                        IsPinned = false
                    },
                    new PublicDiscussion
                    {
                        Id = 3,
                        Title = "Latest Tech Trends",
                        AuthorName = "TechieTom",
                        CreatedAt = DateTime.UtcNow.AddDays(-1),
                        ReplyCount = 4,
                        Category = "Technology",
                        IsPinned = false
                    }
                },
                Categories = new List<ForumCategory>
                {
                    new ForumCategory
                    {
                        Id = 1,
                        Name = "General",
                        Description = "Talk about anything not covered by other categories.",
                        PostCount = 45,
                        IconClass = "fas fa-comments"
                    },
                    new ForumCategory
                    {
                        Id = 2,
                        Name = "Technology",
                        Description = "Discuss the latest in tech.",
                        PostCount = 29,
                        IconClass = "fas fa-microchip"
                    },
                    new ForumCategory
                    {
                        Id = 3,
                        Name = "Feedback",
                        Description = "Suggestions and feedback about the forum.",
                        PostCount = 12,
                        IconClass = "fas fa-bullhorn"
                    }
                }
            };
            return View("Guest", guestModel);
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
