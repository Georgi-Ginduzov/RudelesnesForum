using Forum.Web.Data;
using Forum.Web.Data.Entities;
using Forum.Web.Repositories.Contracts;
using Forum.Web.Repositories.Contracts.Base;

namespace Forum.Web.Repositories.Implementations
{
    public class PostRepository
        : GenericRepository<Post>, IPostRepository
    {
        public PostRepository(ForumDbContext db)
            : base(db) { }


    }
}
