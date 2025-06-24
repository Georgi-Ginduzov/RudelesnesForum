
namespace Forum.Web.Repositories.Contracts.Base
{
    public interface IUnitOfWork : IDisposable
    {
        IPostRepository PostRepository { get; }
        //IRudenessReviewRepository RudenessReviewRepository { get; }
        //IRudenessScanRepository RudenessScanRepository { get; }
        IThreadRepository ThreadRepository { get; }
        IUserRepository UserRepository { get; }

        void SaveChanges();
        Task<int> SaveChangesAsync(CancellationToken ct = default);
    }
}
