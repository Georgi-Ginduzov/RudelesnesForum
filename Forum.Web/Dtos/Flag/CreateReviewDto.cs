using System.ComponentModel.DataAnnotations;

namespace Forum.Web.Dtos.Flag
{
    public class CreateReviewDto
    {
        [Required]
        public bool ShouldBePosted { get; set; }
    }
}
