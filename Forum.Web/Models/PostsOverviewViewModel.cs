namespace Forum.Web.Models
{
    public class PostsOverviewViewModel
    {
        public List<PostSummary> Posts { get; set; } = new List<PostSummary>();
        public List<ForumCategory> Categories { get; set; } = new List<ForumCategory>();
        public PostsFilter Filter { get; set; } = new PostsFilter();
        public PaginationInfo Pagination { get; set; } = new PaginationInfo();
        public int TotalPosts { get; set; }
        public string CurrentSort { get; set; } = "recent";
        public string SearchQuery { get; set; } = "";
    }

    public class PostSummary
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string AuthorName { get; set; }
        public string AuthorAvatar { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LastReplyAt { get; set; }
        public string LastReplyBy { get; set; }
        public int ReplyCount { get; set; }
        public int ViewCount { get; set; }
        public string CategoryName { get; set; }
        public string CategoryColor { get; set; } = "secondary";
        public bool IsPinned { get; set; }
        public bool IsLocked { get; set; }
        public bool HasUnreadReplies { get; set; }
        public List<string> Tags { get; set; } = new List<string>();
    }

    public class PostsFilter
    {
        public int? CategoryId { get; set; }
        public string SortBy { get; set; } = "recent";
        public string TimeFilter { get; set; } = "all";
        public bool ShowPinnedOnly { get; set; } = false;
    }

    public class PaginationInfo
    {
        public int CurrentPage { get; set; } = 1;
        public int TotalPages { get; set; }
        public int PageSize { get; set; } = 20;
        public int TotalItems { get; set; }
        public bool HasPrevious => CurrentPage > 1;
        public bool HasNext => CurrentPage < TotalPages;
    }
}
