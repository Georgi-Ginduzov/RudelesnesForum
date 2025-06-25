using Forum.Web.Data.Entities;

namespace Forum.Web.Models
{
    public class ReplyModerationViewModel
    {
        public DateTime CreatedAt { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public int PostId { get; set; }
        public string PostTitle { get; set; }
        public string Content { get; set; }
        public string FlagReason { get; set; }
        public int Id { get; set; }
    }
}
