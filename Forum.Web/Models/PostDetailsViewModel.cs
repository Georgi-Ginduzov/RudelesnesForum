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
    }

    public class PostDetail
    {
        public int PostId { get; set; }
        public string Title { get; set; } = "";
        public string Content { get; set; } = "";
        public string UserId { get; set; } = "";
        public string AuthorName { get; set; } = "";
        public string AuthorEmail { get; set; } = "";
        public DateTime CreatedAt { get; set; }
        public DateTime LastUpdated { get; set; }
        public int ReplyCount { get; set; }
    }

    public class PostReply
    {
        public int ReplyId { get; set; }
        public string Content { get; set; } = "";
        public string UserId { get; set; } = "";
        public string AuthorName { get; set; } = "";
        public string AuthorEmail { get; set; } = "";
        public DateTime CreatedAt { get; set; }
        public DateTime LastUpdated { get; set; }
    }

    public class PaginationInfo
    {
        public int CurrentPage { get; set; } = 1;
        public int TotalPages { get; set; }
        public int PageSize { get; set; } = 10;
        public int TotalItems { get; set; }
        public bool HasPrevious => CurrentPage > 1;
        public bool HasNext => CurrentPage < TotalPages;
    }
}
