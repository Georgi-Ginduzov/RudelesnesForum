using Forum.Web.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Forum.Web.Data
{
    public class ForumDbContext : DbContext
    {
        public ForumDbContext(DbContextOptions options) : base(options) { }

        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Entities.Thread> Threads { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<RudenessScan> RudenessScans { get; set; }
        public DbSet<RudenessReview> RudenessReviews { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<RudenessReview>()
                        .HasKey(r => new { r.RudenessScanId, r.ReviewerId });

            // Post self-parent check
            modelBuilder.Entity<Post>()
                        .ToTable(p => p.HasCheckConstraint("CK_Post_NoSelfParent", "[ParentId] IS NULL OR [ParentId] <> [PostId]"));

            modelBuilder.Entity<Post>()
                        .HasIndex(p => p.ThreadId);
            modelBuilder.Entity<Post>()
                        .HasIndex(p => p.ParentId);

            modelBuilder.Entity<User>()
                        .HasMany(u => u.Posts)
                        .WithOne(p => p.Creator)
                        .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
