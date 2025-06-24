namespace Forum.Web.Models
{
    public class PostsOverviewViewModel
    {
        public List<PostSummary> Posts { get; set; } = new List<PostSummary>();
        public PaginationInfo Pagination { get; set; } = new PaginationInfo();
        public int TotalPosts { get; set; }
        public string SearchQuery { get; set; } = "";
    }

    public class PostSummary
    {
        public int PostId { get; set; }
        public string Title { get; set; } = "";
        public string Content { get; set; } = "";
        public string UserId { get; set; } = "";
        public string AuthorName { get; set; } = "";
        public string AuthorEmail { get; set; } = "";
        public DateTime CreatedAt { get; set; }
        public DateTime LastUpdated { get; set; }
        public DateTime? LastReplyAt { get; set; }
        public string LastReplyBy { get; set; } = "";
        public int ReplyCount { get; set; }
    }
}
