using Forum.Web.Data;
using Forum.Web.Data.Entities;
using Forum.Web.Services.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Forum.Web.Services
{
    public class ModerationService : IModerationService
    {
        private readonly ApplicationDbContext _dbContext;

        public ModerationService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Reply>> GetFlaggedReplies()
        {
            var flaggedReplies = await _dbContext.Replies
               .Include(r => r.User)
               .Include(r => r.Post)
               .Where(r => r.IsFlagged && !r.IsReviewed)
               .OrderBy(r => r.CreatedAt)
               .ToListAsync();
            return flaggedReplies;
        }

        public async Task ReviewReply(int replyId, bool approved)
        {
            var reply = await _dbContext.Replies.FindAsync(replyId);
            if (reply is null) return;

            if (approved)
            {
                reply.IsReviewed = true;
                reply.IsFlagged = false;
            }
            else
            {
                _dbContext.Remove(reply);
            }
            await _dbContext.SaveChangesAsync();    
        }
    }
}
