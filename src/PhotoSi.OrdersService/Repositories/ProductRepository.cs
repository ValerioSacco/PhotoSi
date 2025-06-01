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
        //private readonly OrdersDbContext _dbContext;
        //private readonly DbSet<Product> _products;

        public ProductRepository(OrdersDbContext dbContext)
            : base(dbContext)
        {
            //_dbContext = dbContext;
            //_products = dbContext.Products;
        }

        public async Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var product = await _dbSet
                .AsNoTracking()
                .Where(u => u.IsAvailable)
                .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);

            return product;
        }

        public bool Create(Product product)
        {
            var created = _dbSet
                .Add(product);

            return created is not null ? true : false;
        }

        //public bool Update(Product product)
        //{
        //    var updated = _products
        //        .Update(product);

        //    return updated is not null ? true : false;
        //}

        //public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        //{
        //    return await _dbContext
        //        .SaveChangesAsync(cancellationToken);
        //}
    }
}
