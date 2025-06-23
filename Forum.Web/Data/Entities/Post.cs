using Forum.Web.Data.Entities.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Forum.Web.Data.Entities
{
    public class Post
    {
        [Key]
        public long PostId { get; set; }

        [ForeignKey(nameof(Thread))]
        public long ThreadId { get; set; }
        public Thread Thread { get; set; } = new Thread();

        [ForeignKey(nameof(Parent))]
        public long? ParentId { get; set; }
        public Post Parent { get; set; } = new Post();

        public ICollection<Post> Replies { get; set; } = [];

        [ForeignKey(nameof(Creator))]
        public int CreatorId { get; set; }
        public User Creator { get; set; } = new User();

        [Required]
        public string Content { get; set; } = default!;

        public PostStatus Status { get; set; } = PostStatus.Pending;

        public DateTime CreatedAt { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}
