namespace Forum.Web.Models
{
    public class UserManagementPageViewModel
    {
        public List<UserManagementViewModel> Users { get; set; } = new List<UserManagementViewModel>();
        public List<string> AvailableRoles { get; set; } = new List<string> { "User", "Moderator", "Admin" };
        public int TotalUsers { get; set; }
        public int AdminCount { get; set; }
        public int ModeratorCount { get; set; }
        public int RegularUserCount { get; set; }
    }

    public class UserManagementViewModel
    {
        public string Id { get; set; } = "";
        public string Email { get; set; } = "";
        public string UserName { get; set; } = "";
        public List<string> Roles { get; set; } = new List<string>();
    }
}
