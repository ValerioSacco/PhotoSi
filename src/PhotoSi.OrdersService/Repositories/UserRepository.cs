using Microsoft.EntityFrameworkCore;
using PhotoSi.OrdersService.Database;
using PhotoSi.OrdersService.Models;
using PhotoSi.Shared.Repositories;

namespace PhotoSi.OrdersService.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        bool Create(User user);
        bool Update(User user);
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }


    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(OrdersDbContext dbContext)
            : base(dbContext)
        {
        }

        public async Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var user = await _dbSet
                .AsNoTracking()
                .Where(u => u.IsAvailable)
                .Include(u => u.Address)
                .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);

            return user;
        }

    }
}
