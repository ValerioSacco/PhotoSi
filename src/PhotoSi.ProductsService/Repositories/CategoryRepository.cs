using Microsoft.EntityFrameworkCore;
using PhotoSi.ProductsService.Database;
using PhotoSi.ProductsService.Models;

namespace PhotoSi.ProductsService.Repositories
{
    public interface ICategoryRepository
    {
        Task<Category?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    }


    public class CategoryRepository : ICategoryRepository
    {
        private readonly ProductsDbContext _dbContext;
        private readonly DbSet<Category> _categories;

        public CategoryRepository(ProductsDbContext dbContext)
        {
            _dbContext = dbContext;
            _categories = dbContext.Categories;
        }


        public async Task<Category?> GetByIdAsync(
            Guid id, 
            CancellationToken cancellationToken
        )
        {
            var category = await _categories
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

            return category;
        }
    }
}
