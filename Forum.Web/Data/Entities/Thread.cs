using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Forum.Web.Data.Entities
{
    public class Thread
    {
        [Key]
        public long ThreadId { get; set; }

        [ForeignKey(nameof(Creator))]
        public int CreatorId { get; set; }
        public ApplicationUser Creator { get; set; }

        [Required, MaxLength(255)]
        public string Topic { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime LastUpdated { get; set; }

        public ICollection<Post> Posts { get; set; }
    }
}
