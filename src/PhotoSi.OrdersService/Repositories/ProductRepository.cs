using Microsoft.EntityFrameworkCore;
using PhotoSi.OrdersService.Database;
using PhotoSi.OrdersService.Models;
using PhotoSi.Shared.Repositories;

namespace PhotoSi.OrdersService.Repositories
{
    public interface IProductRepository
    {
        Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        bool Create(Product product);
        bool Update(Product product);
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }

    public class ProductRepository : BaseRepository<Product>, IProductRepository
    {
        public ProductRepository(OrdersDbContext dbContext)
            : base(dbContext)
        {
        }

        public override async Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var product = await _dbSet
                .Where(u => u.IsAvailable)
                .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);

            return product;
        }
    }
}
