using Forum.Web.Data;
using Forum.Web.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;
using Forum.Web.Repositories.Contracts.Base;
using Forum.Web.Services.Contracts;
using Forum.Web.Services;
using Forum.Web.Repositories.Implementations;

namespace Forum.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException();
            builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));

            builder.Services
                .AddScoped<IPostRepository, PostRepository>()
                .AddScoped<IThreadRepository, ThreadRepository>()
                .AddScoped<IUserRepository, UserRepository>();

            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            builder.Services
                .AddScoped<IPostService, PostService>();

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
