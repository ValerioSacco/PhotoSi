using Microsoft.EntityFrameworkCore;
using PhotoSi.OrdersService.Database;
using PhotoSi.OrdersService.Models;

namespace PhotoSi.OrdersService.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    }


    public class UserRepository : IUserRepository
    {
        private readonly OrdersDbContext _dbContext;
        private readonly DbSet<User> _users;

        public UserRepository(OrdersDbContext dbContext)
        {
            _dbContext = dbContext;
            _users = dbContext.Users;
        }

        public async Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var user = await _users
                .AsNoTracking()
                .Where(u => u.IsAvailable)
                .Include(u => u.Address)
                .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);

            return user;
        }
    }
}
