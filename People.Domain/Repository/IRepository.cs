namespace People.Domain.Repository
{
    using System.Linq;

    public interface IRepository<T, in TKey>
    {
        IQueryable<T> All { get; }
        T Find(TKey id);
        void Update(T entity);
        void Delete(T entity);
        void Create(T entity);
    }
}