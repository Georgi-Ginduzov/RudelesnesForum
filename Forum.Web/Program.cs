using Forum.Web.Configuration;
using Forum.Web.Data;
using Forum.Web.Data.Entities;
using Forum.Web.Repositories.Contracts;
using Forum.Web.Repositories.Contracts.Base;
using Forum.Web.Repositories.Implementations;
using Forum.Web.Services;
using Forum.Web.Services.Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Forum.Web
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException();
            builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));

            builder.Services.Configure<ContentModerationConfiguration>(builder.Configuration.GetSection("ContentModeration"));

            builder.Services
                .AddScoped<IPostRepository, PostRepository>()
                .AddScoped<IThreadRepository, ThreadRepository>()
                .AddScoped<IUserRepository, UserRepository>();

            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            builder.Services
                .AddScoped<IPostService, PostService>()
                .AddScoped<IModerationService, ModerationService>()
                .AddSingleton<IContentModerationService, ContentModerationService>();

            builder.Services
                .AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders()
                .AddDefaultUI();

            builder.Services.AddControllersWithViews();
            builder.Services.AddRazorPages();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthorization();
            app.MapStaticAssets();


            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.MapRazorPages()
               .WithStaticAssets();

            using var scope = app.Services.CreateScope();
            await DbInitializer.SeedAsync(scope.ServiceProvider);

            app.Run();
        }
    }
}
