using Microsoft.EntityFrameworkCore;
using PhotoSi.ProductsService.Database;
using PhotoSi.ProductsService.Models;
using PhotoSi.Shared.Repositories;

namespace PhotoSi.ProductsService.Repositories
{

    public interface IProductRepository
    {
        Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<List<Product>> ListAllAsync(int pageNumber, int pageSize, CancellationToken cancellationToken);
        Task<int> CountAsync(CancellationToken cancellationToken);
        bool Create(Product product);
        bool Update(Product product);
        bool Delete(Product product);
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }

    public class ProductRepository : BaseRepository<Product>, IProductRepository
    {
        //private readonly ProductsDbContext _dbContext;
        //private readonly DbSet<Product> _products;

        public ProductRepository(ProductsDbContext dbContext)
            : base(dbContext)
        {
            //_dbContext = dbContext;
            //_products = dbContext.Products;
        }

        public override async Task<Product?> GetByIdAsync(
            Guid id, 
            CancellationToken cancellationToken
        )
        {
            var product = await _dbSet
                .AsNoTracking()
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);

            return product;
        }

        public async Task<List<Product>> ListAllAsync(int pageNumber, int pageSize, CancellationToken cancellationToken)
        {
            var products = await _dbSet
                .AsNoTracking()
                .OrderBy(p => p.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Include(p => p.Category)
                .ToListAsync(cancellationToken);

            return products;
        }

        public async Task<int> CountAsync(CancellationToken cancellationToken)
        {
            return await _dbSet
                .AsNoTracking()
                .CountAsync(cancellationToken);
        }

        //public bool Create(Product product)
        //{
        //    var created = _products
        //        .Add(product);

        //    return created is not null ? true : false;
        //}

        //public bool Update(Product product)
        //{
        //    var updated = _products
        //        .Update(product);

        //    return updated is not null ? true : false;
        //}

        //public bool Delete(Product product)
        //{
        //    var deleted = _products
        //        .Remove(product);

        //    return deleted is not null ? true : false;
        //}

        //public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        //{
        //    return await _dbContext
        //        .SaveChangesAsync(cancellationToken);
        //}

    }
}
