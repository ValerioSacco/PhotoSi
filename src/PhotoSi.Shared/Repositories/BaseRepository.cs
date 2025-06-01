using Microsoft.EntityFrameworkCore;

namespace PhotoSi.Shared.Repositories
{
    public interface IRepository<T>
        where T : class
    {
        Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        bool Create(T entity);
        bool Update(T entity);
        bool Delete(T entity);
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }

    public abstract class BaseRepository<T> : IRepository<T>
        where T : class
    {
        protected readonly DbContext _context;
        protected readonly DbSet<T> _dbSet;

        public BaseRepository(DbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public virtual async Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _dbSet.FindAsync(id, cancellationToken);
        }

        public bool Create(T entity)
        {
            var created = _dbSet.Add(entity);
            return created is not null ? true : false;
        }

        public bool Update(T entity)
        {
            var updated = _dbSet.Update(entity);
            return updated is not null ? true : false;
        }

        public bool Delete(T entity)
        {
            var removed = _dbSet.Update(entity);
            return removed is not null ? true : false;
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
