using Forum.Web.Data;
using Forum.Web.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;
using Forum.Web.Repositories.Contracts.Base;
using Forum.Web.Services.Contracts;
using Forum.Web.Services;
using Forum.Web.Repositories.Implementations;
using Forum.Web.Services.Forum.Web.Services;

namespace Forum.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException();
            builder.Services.AddDbContext<ForumDbContext>(options => options.UseSqlServer(connectionString));

            builder.Services
                .AddScoped<IPostRepository, PostRepository>()
                .AddScoped<IRudenessReviewRepository, RudenessReviewRepository>()
                .AddScoped<IRudenessScanRepository, RudenessScanRepository>()
                .AddScoped<IThreadRepository, ThreadRepository>()
                .AddScoped<IUserRepository, UserRepository>();

            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            builder.Services
                .AddScoped<IThreadService, ThreadService>()
                .AddScoped<IPostService, PostService>()
                .AddScoped<IFlagService, FlagService>();

            builder.Services.AddControllers();
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

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
