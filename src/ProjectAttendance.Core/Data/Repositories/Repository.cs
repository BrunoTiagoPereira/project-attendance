using Microsoft.EntityFrameworkCore;
using ProjectAttendance.Core.DomainObjects;
using System.Linq.Expressions;

namespace ProjectAttendance.Core.Data.Repositories
{
    public abstract class Repository<T> : IRepository<T> where T : AggregateRoot
    {
        protected readonly DbContext _context;

        public Repository(DbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            Set = context.Set<T>();
        }

        public DbSet<T> Set { get; private init; }

        public virtual Task<T?> FindAsync(long id)
        {
            return Set.FindAsync(id).AsTask();
        }

        public virtual Task<T?> FirstOrDefautAsync(Expression<Func<T, bool>> predicate)
        {
            return Set.FirstOrDefaultAsync(predicate);
        }

        public virtual Task<T?> SingleOrDefautAsync(Expression<Func<T, bool>> predicate)
        {
            return Set.SingleOrDefaultAsync(predicate);
        }

        public virtual Task AddAsync(T entity)
        {
            return Set.AddAsync(entity).AsTask();
        }

        public virtual void Update(T entity)
        {
            Set.Update(entity);
        }

        public virtual void Remove(T entity)
        {
            Set.Remove(entity);
        }

        public virtual void RemoveRange(IEnumerable<T> entities)
        {
            Set.RemoveRange(entities);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}