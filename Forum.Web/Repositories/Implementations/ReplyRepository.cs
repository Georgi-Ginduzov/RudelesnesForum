using Forum.Web.Data;
using Forum.Web.Data.Entities;
using Forum.Web.Repositories.Contracts;

namespace Forum.Web.Repositories.Implementations
{
    public class ReplyRepository : GenericRepository<Reply>, IReplyRepository
    {
        public ReplyRepository(ApplicationDbContext db) : base(db)
        {
        }
    }
}
