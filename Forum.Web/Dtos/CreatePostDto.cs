using System.ComponentModel.DataAnnotations;

namespace Forum.Web.Dtos
{
    public class CreatePostDto
    {
        public long? ParentId { get; set; }

        [Required]
        public string Content { get; set; } = default!;
    }
}
