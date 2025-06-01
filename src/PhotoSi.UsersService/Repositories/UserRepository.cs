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
        public UserRepository(UsersDbContext dbContext) 
            : base(dbContext)
        {
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
    }
}
