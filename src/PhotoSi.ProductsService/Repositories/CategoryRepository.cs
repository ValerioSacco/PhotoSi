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
        public CategoryRepository(ProductsDbContext dbContext)
            : base(dbContext)
        {
        }

        public override async Task<Category?> GetByIdAsync(
            Guid id,
            CancellationToken cancellationToken
        )
        {
            var category = await _dbSet
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
