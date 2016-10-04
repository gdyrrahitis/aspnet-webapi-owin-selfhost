namespace People.Domain.Context
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;

    public interface IBaseDbContext
    {
        IDbSet<TU> GetDbSet<TU>() where TU : class;
        void Entry<T>(T entity, Action<DbEntityEntry<T>> stateAction) where T : class;
    }
}