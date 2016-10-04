namespace People.Domain.Repository
{
    using System.Data.Entity;
    using System.Linq;

    public class Repository<T, TKey>: IRepository<T, TKey> where T: class
    {
        private readonly DbContext _context;

        public Repository(DbContext context)
        {
            _context = context;
        }

        public IQueryable<T> All {
            get { return _context.Set<T>().Select(s => s); }
        }

        public T Find(TKey id)
        {
            return _context.Set<T>().Find(id);
        }

        public void Update(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void Delete(T entity)
        {
            _context.Entry(entity).State = EntityState.Deleted;
            _context.SaveChanges();
        }
    }
}