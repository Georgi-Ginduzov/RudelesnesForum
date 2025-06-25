using Forum.Web.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
namespace Forum.Web.Data
{

    public static class DbInitializer
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();

            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            context.Database.Migrate();
            string[] roles = { "Admin", "Moderator", "User" };

            // 1. Roles
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole(role));
            }

            // 2. Users
            var admin = await CreateUserWithRole(userManager, "admin@example.com", "Admin123!", "Admin");
            var moderator = await CreateUserWithRole(userManager, "mod@example.com", "Mod123!", "Moderator");
            var user = await CreateUserWithRole(userManager, "user@example.com", "User123!", "User");
            var user2 = await CreateUserWithRole(userManager, "user2@example.com", "User123!", "User");
        }

        private static async Task<ApplicationUser> CreateUserWithRole(UserManager<ApplicationUser> userManager, string email, string password, string role = null)
        {
            var existing = await userManager.FindByEmailAsync(email);
            if (existing != null)
                return existing;

            var user = new ApplicationUser
            {
                UserName = email,
                Email = email,
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(user, password);
            if (result.Succeeded && !string.IsNullOrEmpty(role))
            {
                await userManager.AddToRoleAsync(user, role);
            }

            return user;
        }
    }

}
