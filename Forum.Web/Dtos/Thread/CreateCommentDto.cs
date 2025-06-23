using System.ComponentModel.DataAnnotations;

namespace Forum.Web.Dtos.Thread
{
    public class CreateCommentDto
    {
        public long? ParentCommentId { get; set; }
        [Required]
        public string Content { get; set; } = default!;
    }
}
