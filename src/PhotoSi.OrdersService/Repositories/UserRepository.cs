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
        //private readonly OrdersDbContext _dbContext;
        //private readonly DbSet<User> _users;

        public UserRepository(OrdersDbContext dbContext)
            : base(dbContext)
        {
            //_dbContext = dbContext;
            //_users = dbContext.Users;
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

        //public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        //{
        //    return await _dbContext
        //        .SaveChangesAsync(cancellationToken);
        //}
    }
}
