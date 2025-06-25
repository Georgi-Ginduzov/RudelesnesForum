using Forum.Web.Data.Entities;

namespace Forum.Web.Services.Contracts
{
    public interface IModerationService
    {
        Task<IEnumerable<Reply>> GetFlaggedReplies();
        Task<bool> ReviewReply(int replyId, bool approved);
    }
}
