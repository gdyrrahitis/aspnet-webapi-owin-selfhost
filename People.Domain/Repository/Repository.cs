namespace People.Domain.Repository
{
    using System.Data.Entity;
    using System.Linq;
    using Context;

    public class Repository<T, TKey>: IRepository<T, TKey> where T: class
    {
        private readonly BaseDbContext _context;

        public Repository(BaseDbContext context)
        {
            _context = context;
        }

        public IQueryable<T> All {
            get { return _context.GetDbSet<T>().Select(s => s); }
        }

        public T Find(TKey id)
        {
            var entity = _context.GetDbSet<T>();
            return entity.Find(id);
        }

        public void Update(T entity)
        {
            _context.Entry(entity, entry => entry.State = EntityState.Modified);
            _context.SaveChanges();
        }

        public void Delete(T entity)
        {
            _context.Entry(entity, entry => entry.State = EntityState.Deleted);
            _context.SaveChanges();
        }
    }
}