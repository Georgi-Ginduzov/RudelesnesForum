namespace Forum.Web.Models
{
    public class PostDetailsViewModel
    {
        public PostDetail Post { get; set; } = new PostDetail();
        public List<PostReply> Replies { get; set; } = new List<PostReply>();
        public PaginationInfo ReplyPagination { get; set; } = new PaginationInfo();
        public bool CanReply { get; set; } = true;
        public bool CanEdit { get; set; } = false;
        public bool CanDelete { get; set; } = false;
        public bool CanModerate { get; set; } = false;
        public string CurrentUserId { get; set; } = "";
        public bool IsLoggedIn { get; set; } = false;
        public List<PostDetail> RelatedPosts { get; set; } = new List<PostDetail>();
    }

    public class PostDetail
    {
        public int Id { get; set; }
        public string Title { get; set; } = "";
        public string Content { get; set; } = "";
        public string AuthorName { get; set; } = "";
        public string AuthorId { get; set; } = "";
        public string AuthorAvatar { get; set; } = "";
        public string AuthorRole { get; set; } = "Member";
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int ViewCount { get; set; }
        public int ReplyCount { get; set; }
        public string CategoryName { get; set; } = "";
        public int CategoryId { get; set; }
        public string CategoryColor { get; set; } = "secondary";
        public bool IsPinned { get; set; }
        public bool IsLocked { get; set; }
        public List<string> Tags { get; set; } = new List<string>();
        public int LikeCount { get; set; }
        public bool IsLikedByCurrentUser { get; set; }
    }

    public class PostReply
    {
        public int Id { get; set; }
        public string Content { get; set; } = "";
        public string AuthorName { get; set; } = "";
        public string AuthorId { get; set; } = "";
        public string AuthorAvatar { get; set; } = "";
        public string AuthorRole { get; set; } = "Member";
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int LikeCount { get; set; }
        public bool IsLikedByCurrentUser { get; set; }
        public bool IsAuthor { get; set; }
        public int? ParentReplyId { get; set; }
        public string ParentAuthorName { get; set; } = "";
    }
}
