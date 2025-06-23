using System.ComponentModel.DataAnnotations;

namespace Forum.Web.Dtos.Thread
{
    public class CreateThreadDto
    {
        [Required]
        public string Title { get; set; } = default!;
        [Required]
        public string Content { get; set; } = default!;
        public string Topic { get; internal set; } = default!;
    }
}
