namespace Forum.Web.Models
{
    public class GuestHomeViewModel
    {
        public List<PublicDiscussion> RecentDiscussions { get; set; } = new List<PublicDiscussion>();
        public List<ForumCategory> Categories { get; set; } = new List<ForumCategory>();
        public int TotalPosts { get; set; }
        public int TotalMembers { get; set; }
        public int OnlineUsers { get; set; }
        public string ForumName { get; set; } = "Community Forum";
        public string ForumDescription { get; set; } = "Join our community and start engaging in meaningful discussions";
    }

    public class PublicDiscussion
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string AuthorName { get; set; }
        public DateTime CreatedAt { get; set; }
        public int ReplyCount { get; set; }
        public string Category { get; set; }
        public bool IsPinned { get; set; }
    }

    public class ForumCategory
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int PostCount { get; set; }
        public string IconClass { get; set; } = "fas fa-folder";
    }
}
