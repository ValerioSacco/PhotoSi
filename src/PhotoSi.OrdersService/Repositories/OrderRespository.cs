using Microsoft.EntityFrameworkCore;
using PhotoSi.OrdersService.Database;
using PhotoSi.OrdersService.Models;

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

    public class OrderRepository : IOrderRepository
    {
        private readonly OrdersDbContext _dbContext;
        private readonly DbSet<Order> _orders;

        public OrderRepository(OrdersDbContext dbContext)
        {
            _dbContext = dbContext;
            _orders = dbContext.Orders;
        }

        public async Task<Order?> GetByIdAsync(
            Guid id, 
            CancellationToken cancellationToken
        )
        {
            var order = await _orders
                .AsNoTracking()
                .Include(o => o.OrderLines)
                .ThenInclude(ol => ol.Product)
                .Include(o => o.User)
                .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);

            return order;
        }

        public async Task<List<Order>> ListAllAsync(int pageNumber, int pageSize, CancellationToken cancellationToken)
        {
            var orders = await _orders
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
            return await _orders
                .AsNoTracking()
                .CountAsync(cancellationToken);
        }

        public bool Create(Order order)
        {
            var created = _orders
                .Add(order);

            return created is not null ? true : false;
        }

        public bool Update(Order order)
        {
            var updated = _orders
                .Update(order);

            return updated is not null ? true : false;
        }

        public bool Delete(Order order)
        {
            var deleted = _orders
                .Remove(order);

            return deleted is not null ? true : false;
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            return await _dbContext
                .SaveChangesAsync(cancellationToken);
        }

    }
}
