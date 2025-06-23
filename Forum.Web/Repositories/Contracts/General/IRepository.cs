namespace Forum.Web.Repositories.Contracts.Base
{
    public interface IRepository<T> where T : class
    {
        IQueryable<T> Entities { get; }
        void Add(T entity);
        void Update(T entity);
        void Remove(T entity);
    }
}
