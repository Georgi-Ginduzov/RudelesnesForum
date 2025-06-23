using Forum.Web.Dtos.Thread;
using Forum.Web.Dtos;

namespace Forum.Web.Services.Contracts
{
    public interface IPostService
    {
        Task<(IEnumerable<PostDto> Posts, string? NextCursor)>
        GetForThreadAsync(long threadId, int limit, DateTime? before);

        Task<long>
            AddAsync(int creatorId, long threadId, CreatePostDto dto);

    }
}
