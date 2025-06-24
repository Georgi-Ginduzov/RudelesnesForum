using Microsoft.AspNetCore.Identity;

namespace Forum.Web.Data.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public ICollection<Post> Posts { get; set; }
        public ICollection<Reply> Replies { get; set; }
    }
}
