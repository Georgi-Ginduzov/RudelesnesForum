using Forum.Web.Dtos.Thread;

namespace Forum.Web.Services.Contracts
{
    public interface IThreadService
    {
        Task<(IEnumerable<ThreadSummaryDto> Threads, string? NextCursor)>
        GetRecentAsync(int limit, DateTime? before);

        Task<ThreadDetailDto?>
            GetByIdAsync(long threadId, int postLimit, DateTime? beforePosts);

        Task<long>
            CreateAsync(int creatorId, CreateThreadDto dto);
    }
}
