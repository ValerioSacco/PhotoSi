using Microsoft.EntityFrameworkCore;

namespace PhotoSi.ProductsService.Repositories
{

    public interface IRepository<T> where T : class
    {
        Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken);
        Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken);
        Task AddAsync(T entity, CancellationToken cancellationToken);
        void Update(T entity);
        void Delete(T entity);

    }


    public abstract class BaseRepository<T> : IRepository<T> where T : class
    {
        protected readonly DbContext _context;
        protected readonly DbSet<T> _dbSet;

        public BaseRepository(DbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public virtual async Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken) 
            => await _dbSet.FindAsync(id, cancellationToken);
        public virtual async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken) 
            => await _dbSet.ToListAsync(cancellationToken);
        public virtual async Task AddAsync(T entity, CancellationToken cancellationToken) 
            => await _dbSet.AddAsync(entity, cancellationToken);
        public virtual void Update(T entity) 
            => _dbSet.Update(entity);
        public virtual void Delete(T entity) 
            => _dbSet.Remove(entity);

    }
}
