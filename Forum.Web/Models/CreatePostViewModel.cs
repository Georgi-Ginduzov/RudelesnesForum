using System.ComponentModel.DataAnnotations;

namespace Forum.Web.Models
{
    public class CreatePostViewModel
    {
        [Required(ErrorMessage = "Title is required")]
        [StringLength(200, ErrorMessage = "Title cannot exceed 200 characters")]
        public string Title { get; set; } = "";

        [Required(ErrorMessage = "Content is required")]
        [StringLength(10000, ErrorMessage = "Content cannot exceed 10,000 characters")]
        public string Content { get; set; } = "";
    }
}
