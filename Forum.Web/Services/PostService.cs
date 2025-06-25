using Forum.Web.Data.Entities;
using Forum.Web.Data;
using Forum.Web.Services.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Forum.Web.Services
{
    public class PostService : IPostService
    {
        private readonly ApplicationDbContext db;
        private readonly IContentModerationService contentModerationService;
        public PostService(ApplicationDbContext db, IContentModerationService contentModerationService)
        {
            this.db = db;
            this.contentModerationService = contentModerationService;
        }

        public async Task<IEnumerable<Post>> GetAllPostsAsync(string search = "", int skip = 0, int take = 0)
        {
            var postsCount = await db.Posts.CountAsync();
            var allPosts = await db.Posts
                .Include(x => x.User)
                .Include(x => x.Replies).ThenInclude(x => x.User)
                .Skip(skip)
                .Take(take == 0 ? postsCount : take)
                .ToListAsync();

            if (!string.IsNullOrEmpty(search))
            {
                var filteredPosts = allPosts
                    .Where(x => x.Title.Contains(search) || x.Content.Contains(search))
                    .ToList();
                return filteredPosts;
            }
            return allPosts;
        }

        public async Task<int> GetPostsCountAsync() => await db.Posts.CountAsync();

        public async Task<Post> GetPostByIdAsync(int postId)
        {
            var post = await db.Posts
                .Include(x => x.User)
                .Include(x => x.Replies)
                .FirstOrDefaultAsync(x => x.PostId == postId);
            if (post == null) throw new ArgumentException($"Invalid post id: {postId}");
            return post;
        }

        public async Task<int> CreateAsync(string creatorId, string title, string content)
        {
            var post = new Post()
            {
                Title = title,
                Content = content,
                UserId = creatorId,
                CreatedAt = DateTime.UtcNow,
                LastUpdated = DateTime.UtcNow,
            };
           
            var postResult = await db.Posts.AddAsync(post);
            await db.SaveChangesAsync();
            return postResult.Entity.PostId;
        }

        public async Task<(int replyId, bool isFlagged)> AddPostReplyAsync(string creatorId, int postId, string reply)
        {
            var shouldFlag = contentModerationService.IsRudeAsync(reply);
            var replyEntity = new Reply
            {
                Content = reply,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsFlagged = shouldFlag,
                IsReviewed = !shouldFlag,
                PostId = postId,
                UserId = creatorId
            };

            var replyResult = await db.Replies.AddAsync(replyEntity);
            await db.SaveChangesAsync();
            return (replyResult.Entity.Id, shouldFlag);
        }

        public async Task<int?> DeleteReplyAsync(int replyId)
        {
            var reply = await db.Replies.FindAsync(replyId);
            db.Replies.Remove(reply);
            await db.SaveChangesAsync();
            return reply.PostId;
        }
    }
}
