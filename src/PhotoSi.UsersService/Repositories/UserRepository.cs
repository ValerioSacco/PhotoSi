using Microsoft.EntityFrameworkCore;
using PhotoSi.Shared.Repositories;
using PhotoSi.UsersService.Database;
using PhotoSi.UsersService.Models;

namespace PhotoSi.UsersService.Repositories
{

    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<List<User>> ListAllAsync(int pageNumber, int pageSize, CancellationToken cancellationToken);
        Task<int> CountAsync(CancellationToken cancellationToken);
        bool Create(User user);
        bool Update(User user);
        bool Delete(User user);
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }

    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        //private readonly UsersDbContext _dbContext;
        //private readonly DbSet<User> _users;

        public UserRepository(UsersDbContext dbContext) 
            : base(dbContext)
        {
            //_dbContext = dbContext;
            //_users = dbContext.Users;
        }

        public override async Task<User?> GetByIdAsync(
            Guid id,
            CancellationToken cancellationToken
        )
        {
            var user = await _dbSet
                .AsNoTracking()
                .Include(u => u.ShipmentAddress)
                .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);

            return user;
        }

        public async Task<List<User>> ListAllAsync(int pageNumber, int pageSize, CancellationToken cancellationToken)
        {
            var users = await _dbSet
                .AsNoTracking()
                .OrderBy(u => u.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Include(u => u.ShipmentAddress)
                .ToListAsync(cancellationToken);

            return users;
        }

        public async Task<int> CountAsync(CancellationToken cancellationToken)
        {
            return await _dbSet
                .AsNoTracking()
                .CountAsync(cancellationToken);
        }

        //public bool Create(User user)
        //{
        //    var created = _users
        //        .Add(user);

        //    return created is not null ? true : false;
        //}

        //public bool Update(User user)
        //{
        //    var updated = _users
        //        .Update(user);

        //    return updated is not null ? true : false;
        //}

        //public bool Delete(Product product)
        //{
        //    var deleted = _users
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
