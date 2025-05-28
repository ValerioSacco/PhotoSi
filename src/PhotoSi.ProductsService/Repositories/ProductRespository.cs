using Microsoft.EntityFrameworkCore;
using PhotoSi.ProductsService.Database;
using PhotoSi.ProductsService.Models;

namespace PhotoSi.ProductsService.Repositories
{
    //public interface IProductRespository : IRepository<Product>;

    //public class ProductRespository : BaseRepository<Product>, IProductRespository
    //{
    //    public ProductRespository(ProductsDbContext context) 
    //        : base(context)
    //    {
    //    }

    //    public override Task<Product?> GetByIdAsync(int id, CancellationToken cancellationToken)
    //    {
    //        return _dbSet
    //            .AsNoTracking()
    //            .Include(p => p.Category)
    //            .FirstOrDefaultAsync(p => p.Code == id);      
    //    }

    //}

    public interface IProductRepository
    {
        Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<int> CreateAsync(Product product, CancellationToken cancellationToken);
    }

    public class ProductRepository : IProductRepository
    {
        private readonly DbSet<Product> _products;

        public ProductRepository(ProductsDbContext dbContext)
        {
            _products = dbContext.Products;
        }

        public async Task<Product?> GetByIdAsync(
            Guid id, 
            CancellationToken cancellationToken
        )
        {
            var product = await _products
                .AsNoTracking()
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id == id);

            return product;
        }

        public async Task<int> CreateAsync(
            Product product, 
            CancellationToken cancellationToken
        )
        {
            var created = await _products
                .AddAsync(product);

            return 1;
        }

    }
}
