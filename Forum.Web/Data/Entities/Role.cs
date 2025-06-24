using System.ComponentModel.DataAnnotations;

namespace Forum.Web.Data.Entities
{
    public class Role
    {
        [Key]
        public int RoleId { get; set; }

        [Required, MaxLength(50)]
        public string Name { get; set; }

        public string Description { get; set; }

        public ICollection<ApplicationUser> Users { get; set; }
    }
}
