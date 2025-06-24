namespace Forum.Web.Data.Entities
{
    public class Reply
    {
        public int Id { get; set; }

        public string Content { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        public bool IsFlagged { get; set; } = false; 
        public bool IsReviewed { get; set; } = false;

        public int? PostId { get; set; }
        public Post Post { get; set; }

        public int? ParentReplyId { get; set; }
        public Reply ParentReply { get; set; }

        public ICollection<Reply> Replies { get; set; } = new List<Reply>();

        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}
