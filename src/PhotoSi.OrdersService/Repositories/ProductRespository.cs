using Microsoft.EntityFrameworkCore;
using PhotoSi.OrdersService.Database;
using PhotoSi.OrdersService.Models;

namespace PhotoSi.OrdersService.Repositories
{
    public interface IProductRepository
    {
        Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    }

    public class ProductRepository : IProductRepository
    {
        private readonly OrdersDbContext _dbContext;
        private readonly DbSet<Product> _products;

        public ProductRepository(OrdersDbContext dbContext)
        {
            _dbContext = dbContext;
            _products = dbContext.Products;
        }

        public async Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var product = await _products
                .AsNoTracking()
                .Where(u => u.IsAvailable)
                .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);

            return product;
        }
    }
}
