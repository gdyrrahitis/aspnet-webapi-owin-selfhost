namespace People.Domain.Repository
{
    using System.Data.Entity;
    using System.Linq;

    public interface IRepository<T, in TKey>
    {
        IQueryable<T> All { get; }

        T Find(TKey id);

        void Update(T entity);

        void Delete(T entity);
    }
}