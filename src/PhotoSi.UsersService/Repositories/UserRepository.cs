using Microsoft.EntityFrameworkCore;
using PhotoSi.UsersService.Database;
using PhotoSi.UsersService.Models;


namespace PhotoSi.UsersService.Repositories
{

    public interface IUserRepository
    {
        bool Create(User user);
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }

    public class UserRepository : IUserRepository
    {
        private readonly UsersDbContext _dbContext;
        private readonly DbSet<User> _users;

        public UserRepository(UsersDbContext dbContext)
        {
            _dbContext = dbContext;
            _users = dbContext.Users;
        }

        //public async Task<Product?> GetByIdAsync(
        //    Guid id,
        //    CancellationToken cancellationToken
        //)
        //{
        //    var product = await _users
        //        .AsNoTracking()
        //        .Include(p => p.Category)
        //        .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);

        //    return product;
        //}

        //public async Task<List<Product>> ListAllAsync(int pageNumber, int pageSize, CancellationToken cancellationToken)
        //{
        //    var products = await _users
        //        .AsNoTracking()
        //        .OrderBy(p => p.Id)
        //        .Skip((pageNumber - 1) * pageSize)
        //        .Take(pageSize)
        //        .Include(p => p.Category)
        //        .ToListAsync(cancellationToken);

        //    return products;
        //}

        //public async Task<int> CountAsync(CancellationToken cancellationToken)
        //{
        //    return await _users
        //        .AsNoTracking()
        //        .CountAsync(cancellationToken);
        //}

        public bool Create(User user)
        {
            var created = _users
                .Add(user);

            return created is not null ? true : false;
        }

        //public bool Update(Product product)
        //{
        //    var updated = _users
        //        .Update(product);

        //    return updated is not null ? true : false;
        //}

        //public bool Delete(Product product)
        //{
        //    var deleted = _users
        //        .Remove(product);

        //    return deleted is not null ? true : false;
        //}

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            return await _dbContext
                .SaveChangesAsync(cancellationToken);
        }

    }
}
