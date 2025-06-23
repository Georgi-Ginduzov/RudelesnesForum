using Forum.Web.Data;
using Forum.Web.Data.Entities;
using Forum.Web.Repositories.Contracts;

namespace Forum.Web.Repositories.Implementations
{
    public class RudenessScanRepository : GenericRepository<RudenessScan>, IRudenessScanRepository
    {
        public RudenessScanRepository(ForumDbContext db)
            : base(db) 
        { 
        
        }

    }
}
