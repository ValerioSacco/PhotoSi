using Microsoft.EntityFrameworkCore;
using PhotoSi.ProductsService.Database;
using PhotoSi.ProductsService.Models;
using PhotoSi.Shared.Repositories;

namespace PhotoSi.ProductsService.Repositories
{
    public interface ICategoryRepository
    {
        Task<Category?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<List<Category>> ListAllAsync(CancellationToken cancellationToken);
    }


    public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
    {
        //private readonly ProductsDbContext _dbContext;
        //private readonly DbSet<Category> _categories;

        public CategoryRepository(ProductsDbContext dbContext) 
            : base(dbContext)
        {
            //_dbContext = dbContext;
            //_categories = dbContext.Categories;
        }


        public override async Task<Category?> GetByIdAsync(
            Guid id, 
            CancellationToken cancellationToken
        )
        {
            var category = await _dbSet
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

            return category;
        }

        public async Task<List<Category>> ListAllAsync(CancellationToken cancellationToken)
        {
            var categories = await _dbSet
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            return categories;
        }
    }
}
