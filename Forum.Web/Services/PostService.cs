using Forum.Web.Data.Entities.Enums;
using Forum.Web.Data.Entities;
using Forum.Web.Data;
using Forum.Web.Dtos;
using Forum.Web.Services.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Forum.Web.Services
{
    public class PostService : IPostService
    {
        private readonly ForumDbContext db;
        public PostService(ForumDbContext db) => this.db = db;

        public async Task<(IEnumerable<PostDto>, string?)> GetForThreadAsync(long threadId, int limit, DateTime? before)
        {
            var cutoff = before ?? DateTime.UtcNow;
            var q = from p in db.Posts
                    where p.ThreadId == threadId
                       && p.Status == PostStatus.Approved
                       && p.CreatedAt < cutoff
                    orderby p.CreatedAt descending
                    select new PostDto
                    {
                        PostId = p.PostId,
                        Content = p.Content,
                        Creator = p.Creator.NickName,
                        CreatedAt = p.CreatedAt
                    };

            var page = await q.Take(limit + 1).ToListAsync();
            var hasMore = page.Count == limit + 1;
            if (hasMore) page.RemoveAt(limit);

            var nextCursor = hasMore
                ? page.Last().CreatedAt.ToString("o")
                : null;

            return (page, nextCursor);
        }

        public async Task<long> AddAsync(int creatorId, long threadId, CreatePostDto dto)
        {
            var post = new Post
            {
                ThreadId = threadId,
                ParentId = dto.ParentId,
                Content = dto.Content,
                CreatorId = creatorId,
                CreatedAt = DateTime.UtcNow,
                LastUpdated = DateTime.UtcNow,
                Status = PostStatus.Pending
            };
            db.Posts.Add(post);
            await db.SaveChangesAsync();
            return post.PostId;
        }
    }
}
