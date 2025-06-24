using Forum.Web.Data.Entities;

namespace Forum.Web.Services.Contracts
{
    public interface IPostService
    {
        Task<Post> GetPostByIdAsync(int postId);
        Task<int> CreateAsync(string creatorId, string title, string content);
        Task<int> AddPostReplyAsync(string creatorId, int postId, string reply);
        Task<int?> DeleteReplyAsync(int replyId);
    }
}
