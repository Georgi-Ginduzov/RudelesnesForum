using Forum.Web.Data.Entities;
using Forum.Web.Repositories.Contracts.Base;

namespace Forum.Web.Repositories.Contracts
{
    public interface IThreadRepository : IRepository<Data.Entities.Thread>
    {
    }
}
