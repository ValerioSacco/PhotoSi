using Microsoft.EntityFrameworkCore;
using PhotoSi.OrdersService.Database;
using PhotoSi.OrdersService.Models;
using PhotoSi.Shared.Repositories;

namespace PhotoSi.OrdersService.Repositories
{
    public interface IOrderRepository
    {
        Task<Order?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<List<Order>> ListAllAsync(int pageNumber, int pageSize, CancellationToken cancellationToken);
        Task<int> CountAsync(CancellationToken cancellationToken);
        bool Create(Order order);
        bool Update(Order order);
        bool Delete(Order order);
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }

    public class OrderRepository : BaseRepository<Order>, IOrderRepository
    {
        public OrderRepository(OrdersDbContext dbContext)
            : base(dbContext)
        {
        }

        public override async Task<Order?> GetByIdAsync(
            Guid id,
            CancellationToken cancellationToken
        )
        {
            var order = await _dbSet
                .Include(o => o.OrderLines)
                .ThenInclude(ol => ol.Product)
                .Include(o => o.User)
                .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);

            return order;
        }

        public async Task<List<Order>> ListAllAsync(int pageNumber, int pageSize, CancellationToken cancellationToken)
        {
            var orders = await _dbSet
                .AsNoTracking()
                .OrderBy(p => p.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Include(o => o.OrderLines)
                .ThenInclude(ol => ol.Product)
                .Include(o => o.User)
                .ToListAsync(cancellationToken);

            return orders;
        }

        public async Task<int> CountAsync(CancellationToken cancellationToken)
        {
            return await _dbSet
                .AsNoTracking()
                .CountAsync(cancellationToken);
        }

    }
}
