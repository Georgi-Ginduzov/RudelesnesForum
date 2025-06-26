using Forum.Web.Data;
using Forum.Web.Repositories.Contracts.Base;
using Microsoft.EntityFrameworkCore;

namespace Forum.Web.Repositories
{
    public class GenericRepository<T> : IRepository<T> where T : class
    {
        protected readonly ApplicationDbContext db;
        public GenericRepository(ApplicationDbContext db) => this.db = db;

        public IQueryable<T> Entities => db.Set<T>().AsNoTracking();

        public void Add(T entity) => db.Set<T>().Add(entity);
        public void Update(T entity) => db.Set<T>().Update(entity);
        public virtual void Remove(T entity) => db.Set<T>().Remove(entity);
    }
}
