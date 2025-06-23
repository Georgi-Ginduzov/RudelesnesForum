using Forum.Web.Data;
using Forum.Web.Data.Entities;
using Forum.Web.Repositories.Contracts;

namespace Forum.Web.Repositories.Implementations
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(ForumDbContext db)
            : base(db) { }
    }
}
