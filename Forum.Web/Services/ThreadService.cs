//using Forum.Web.Dtos.Thread;
//using Forum.Web.Services.Contracts;

//namespace Forum.Web.Services
//{
//    using global::Forum.Web.Data.Entities;
//    using global::Forum.Web.Data.Entities.Enums;
//    using global::Forum.Web.Dtos;
//    using global::Forum.Web.Repositories.Contracts.Base;
//    using Microsoft.EntityFrameworkCore;

//    namespace Forum.Web.Services
//    {
//        public class ThreadService : IThreadService
//        {
//            private readonly IUnitOfWork _uow;

//            public ThreadService(IUnitOfWork uow) => _uow = uow;

//            public async Task<(IEnumerable<ThreadSummaryDto>, string?)> GetRecentAsync(int limit, DateTime? before)
//            {
//                var cutoff = before ?? DateTime.UtcNow;
//                var query = _uow.ThreadRepository.Entities
//                    .Where(t => t);
//                var q = from t in _uow.ThreadRepository.Entities
//                        where t.Status == ThreadStatus.Approved
//                        orderby t.LastUpdated descending
//                        select new ThreadSummaryDto
//                        {
//                            ThreadId = t.ThreadId,
//                            Topic = t.Topic,
//                            PostCount = t.Posts.Count(p => p.Status == PostStatus.Approved),
//                            LastUpdated = t.LastUpdated
//                        };

//                var page = await q.Take(limit + 1).ToListAsync();
//                var hasMore = page.Count() == limit + 1;
//                if (hasMore) page.RemoveAt(limit);

//                var nextCursor = hasMore
//                    ? page.Last().LastUpdated.ToString("o")
//                    : null;

//                return (page, nextCursor);
//            }

//            public async Task<ThreadDetailDto?> GetByIdAsync(long threadId, int postLimit, DateTime? beforePosts)
//            {
//                var cutoff = beforePosts ?? DateTime.UtcNow;

//                var threadEntity = await _uow.ThreadRepository.Entities
//                    .Include(t => t.Creator)
//                    .FirstOrDefaultAsync(t => t.ThreadId == threadId && t.Status == ThreadStatus.Approved);

//                if (threadEntity == null) return null;

//                // map header
//                var detail = new ThreadDetailDto
//                {
//                    ThreadId = threadEntity.ThreadId,
//                    Topic = threadEntity.Topic,
//                    Creator = threadEntity.Creator.NickName,
//                    CreatedAt = threadEntity.CreatedAt
//                };

//                // page posts
//                var postsQ = _uow.PostRepository.Entities
//                    .Where(p => p.ThreadId == threadId
//                             && p.Status == PostStatus.Approved
//                             && p.CreatedAt < cutoff)
//                    .OrderByDescending(p => p.CreatedAt)
//                    .Take(postLimit + 1)
//                    .Select(p => new PostDto
//                    {
//                        PostId = p.PostId,
//                        Content = p.Content,
//                        Creator = p.Creator.NickName,
//                        CreatedAt = p.CreatedAt
//                    });

//                var postPage = await postsQ.ToListAsync();
//                var hasMore = postPage.Count == postLimit + 1;
//                if (hasMore) postPage.RemoveAt(postLimit);

//                detail.Posts = postPage;
//                detail.NextCursor = hasMore
//                    ? postPage.Last().CreatedAt.ToString("o")
//                    : null;

//                return detail;
//            }

//            public async Task<long> CreateAsync(int creatorId, CreateThreadDto dto)
//            {
//                var thread = new Thread
//                {
//                    Topic = dto.Topic,
//                    CreatorId = creatorId,
//                    CreatedAt = DateTime.UtcNow,
//                    LastUpdated = DateTime.UtcNow,
//                    Status = ThreadStatus.Pending
//                };
//                _uow.ThreadRepository.Add(thread);

//                var firstPost = new Post
//                {
//                    Thread = thread,
//                    Content = dto.Content,
//                    CreatorId = creatorId,
//                    CreatedAt = DateTime.UtcNow,
//                    LastUpdated = DateTime.UtcNow,
//                    Status = PostStatus.Pending
//                };
//                _uow.PostRepository.Add(firstPost);

//                await _uow.SaveChangesAsync();
//                return thread.ThreadId;
//            }
//        }

//    }
//}
