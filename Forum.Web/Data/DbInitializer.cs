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

            // 3. Posts & Replies
            if (!context.Posts.Any())
            {
                var posts = new List<Post>
            {
                new Post
                {
                    Title = "Welcome to the Forum!",
                    Content = "Feel free to introduce yourself and share your interests.",
                    CreatedAt = DateTime.UtcNow.AddDays(-10),
                    LastUpdated = DateTime.UtcNow.AddDays(-5),
                    UserId = user.Id,
                    Replies = new List<Reply>
                    {
                        new Reply
                        {
                            Content = "Happy to be here!",
                            CreatedAt = DateTime.UtcNow.AddDays(-9),
                            UserId = user.Id,
                            IsFlagged = true,
                            IsReviewed = false,
                            UpdatedAt = DateTime.UtcNow,
                        },
                        new Reply
                        {
                            Content = "Hello everyone!",
                            CreatedAt = DateTime.UtcNow.AddDays(-8),
                            UserId = moderator.Id,
                            IsReviewed = false,
                            IsFlagged= false,
                            UpdatedAt= DateTime.UtcNow,
                        }
                    }
                },
                new Post
                {
                    Title = "ML.NET Tips and Tricks",
                    Content = "Let’s share our ML.NET experiences. Best practices?",
                    CreatedAt = DateTime.UtcNow.AddDays(-3),
                    LastUpdated = DateTime.UtcNow,
                    UserId = user.Id,
                    Replies = new List<Reply>
                    {
                        new Reply
                        {
                            Content = "Try using normalized text features!",
                            CreatedAt = DateTime.UtcNow.AddDays(-2),
                            UserId = admin.Id,
                            UpdatedAt = DateTime.UtcNow,
                            Replies = new List<Reply>
                            {
                                new Reply
                                {
                                    Content = "That actually helped me a lot. Thanks!",
                                    CreatedAt = DateTime.UtcNow.AddDays(-1),
                                    UserId = user.Id,
                                    ParentReplyId = null // this will be auto-linked
                                }
                            }
                        }
                    }
                }
            };

                context.Posts.AddRange(posts);
                await context.SaveChangesAsync();
            }
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
