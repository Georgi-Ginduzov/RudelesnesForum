using System.ComponentModel.DataAnnotations;

namespace Forum.Web.Data.Entities
{
    public class Post
    {
        [Key]
        public int PostId { get; set; }

        public string Title { get; set; }

        [Required]
        public string Content { get; set; } = default!;
       
        public DateTime CreatedAt { get; set; }

        public DateTime LastUpdated { get; set; }

        public string UserId { get; set; }

        public ApplicationUser User { get; set; }

        public ICollection<Reply> Replies { get; set; }
    }
}
