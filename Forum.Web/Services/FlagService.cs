using Forum.Web.Data.Entities.Enums;
using Forum.Web.Data.Entities;
using Forum.Web.Data;
using Forum.Web.Dtos.Flag;
using Forum.Web.Services.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Forum.Web.Services
{
    public class FlagService : IFlagService
    {
        private readonly ForumDbContext _db;
        public FlagService(ForumDbContext db) => _db = db;

        public async Task<(IEnumerable<FlagDto>, string?)> GetFlagsAsync(int limit, DateTime? before)
        {
            var cutoff = before ?? DateTime.UtcNow;
            var q = from scan in _db.RudenessScans
                    where scan.Status == ScanStatus.Pending
                       || scan.Status == ScanStatus.Flagged
                    orderby scan.CreatedAt descending
                    select new FlagDto
                    {
                        ScanId = scan.ScanId,
                        PostId = scan.PostId,
                        PostContent = scan.Post.Content,
                        CreatorNick = scan.Post.Creator.NickName,
                        CreatedAt = scan.CreatedAt,
                        RudeLevel = scan.RudeLevel.ToString()
                    };

            var page = await q.Take(limit + 1).ToListAsync();
            var hasMore = page.Count == limit + 1;
            if (hasMore) page.RemoveAt(limit);

            var nextCursor = hasMore
                ? page.Last().CreatedAt.ToString("o")
                : null;

            return (page, nextCursor);
        }

        public async Task<FlagDetailDto?> GetFlagByIdAsync(long scanId)
        {
            var scan = await _db.RudenessScans
                .AsNoTracking()
                .Include(s => s.Post).ThenInclude(p => p.Creator)
                .Include(s => s.Reviews).ThenInclude(r => r.Reviewer)
                .FirstOrDefaultAsync(s => s.ScanId == scanId);

            if (scan == null) return null;

            return new FlagDetailDto
            {
                ScanId = scan.ScanId,
                PostId = scan.PostId,
                PostContent = scan.Post.Content,
                CreatorNick = scan.Post.Creator.NickName,
                CreatedAt = scan.CreatedAt,
                RudeLevel = scan.RudeLevel.ToString(),
                Status = scan.Status,
                Reviews = scan.Reviews.Select(r => new ReviewDto
                {
                    ReviewerId = r.ReviewerId,
                    ReviewerNick = r.Reviewer.NickName,
                    ShouldBePosted = r.ShouldBePosted,
                    ReviewedAt = r.ReviewedAt
                })
            };
        }

        public async Task AddReviewAsync(int reviewerId, long scanId, bool shouldBePosted)
        {
            var scan = await _db.RudenessScans
                .Include(s => s.Reviews)
                .FirstOrDefaultAsync(s => s.ScanId == scanId)
                    ?? throw new KeyNotFoundException("Scan not found");

            // add moderator's review
            var review = new RudenessReview
            {
                RudenessScanId = scanId,
                ReviewerId = reviewerId,
                ShouldBePosted = shouldBePosted,
                ReviewedAt = DateTime.UtcNow
            };
            scan.Reviews.Add(review);

            // update scan status
            scan.Status = shouldBePosted
                ? ScanStatus.Approved
                : ScanStatus.Rejected;
            scan.Post.Status = shouldBePosted
                ? PostStatus.Approved
                : PostStatus.Rejected;

            await _db.SaveChangesAsync();
        }
    }
}
