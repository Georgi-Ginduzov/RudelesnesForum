using Forum.Web.Data;
using Forum.Web.Repositories.Contracts;
using Forum.Web.Repositories.Contracts.Base;

namespace Forum.Web.Repositories.Implementations
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ForumDbContext db;

        public UnitOfWork(ForumDbContext db, IPostRepository postRepository, IRudenessReviewRepository rudenessReviewRepository, IRudenessScanRepository rudenessScanRepository, IThreadRepository threadRepository, IUserRepository userRepository)
        {
            this.db = db;

            PostRepository = postRepository;
            RudenessReviewRepository = rudenessReviewRepository;
            RudenessScanRepository = rudenessScanRepository;
            ThreadRepository = threadRepository;
            UserRepository = userRepository;
        }

        public IPostRepository PostRepository { get; }
        public IRudenessReviewRepository RudenessReviewRepository { get; }
        public IRudenessScanRepository RudenessScanRepository{ get; }
        public IThreadRepository ThreadRepository { get; }
        public IUserRepository UserRepository { get; }

        public void Dispose() => db?.Dispose();

        public Task<int> SaveChangesAsync(CancellationToken ct = default) 
                    => db.SaveChangesAsync(ct);

        public void SaveChanges() => db.SaveChanges();

    }
}
