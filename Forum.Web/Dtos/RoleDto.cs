namespace Forum.Web.Dtos
{
    public class RoleDto
    {
        public int RoleId { get; set; }
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
    }
}
