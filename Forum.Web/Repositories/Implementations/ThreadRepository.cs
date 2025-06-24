using Forum.Web.Data;
using Forum.Web.Data.Entities;
using Forum.Web.Repositories.Contracts;

namespace Forum.Web.Repositories.Implementations
{
    public class ThreadRepository : GenericRepository<Data.Entities.Thread>, IThreadRepository
    {
        public ThreadRepository(ApplicationDbContext db)
            : base(db) 
        { 
        
        }

    }
}
