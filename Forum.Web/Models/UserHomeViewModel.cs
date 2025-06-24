namespace Forum.Web.Models
{
    public class UserHomeViewModel
    {
        public List<LatestDiscussion> LatestDiscussions { get; set; } = new List<LatestDiscussion>();
        public int TotalPosts { get; set; }
        public int TotalUsers { get; set; }
        public int OnlineUsers { get; set; }
        public int TodaysPosts { get; set; }
    }

    public class LatestDiscussion
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string AuthorName { get; set; }
        public DateTime CreatedAt { get; set; }
        public int ReplyCount { get; set; }
        public string LastReplyBy { get; set; }
        public DateTime? LastReplyAt { get; set; }
        public string Category { get; set; }
        public bool IsPinned { get; set; }
        public bool IsLocked { get; set; }
    }
}
