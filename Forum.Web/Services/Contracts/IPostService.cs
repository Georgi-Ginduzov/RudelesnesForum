using Forum.Web.Data.Entities;

namespace Forum.Web.Services.Contracts
{
    public interface IPostService
    {
        Task<IEnumerable<Post>> GetAllPostsAsync(string search = "", int skip = 0, int take = 0);
        Task<int> GetPostsCountAsync();
        Task<Post> GetPostByIdAsync(int postId);
        Task<int> CreateAsync(string creatorId, string title, string content);
        Task<(int replyId, bool isFlagged)> AddPostReplyAsync(string creatorId, int postId, string reply);
        Task DeletePostAsync(int postId);
        Task<int?> DeleteReplyAsync(int replyId);
    }
}
