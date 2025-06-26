
namespace Forum.Web.Repositories.Contracts.Base
{
    public interface IUnitOfWork : IDisposable
    {
        IPostRepository PostRepository { get; }
        //IRudenessReviewRepository RudenessReviewRepository { get; }
        //IRudenessScanRepository RudenessScanRepository { get; }
        IUserRepository UserRepository { get; }
        IReplyRepository ReplyRepository { get; }

        void SaveChanges();
        Task<int> SaveChangesAsync(CancellationToken ct = default);
    }
}
