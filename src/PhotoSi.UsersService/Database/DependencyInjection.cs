using Microsoft.EntityFrameworkCore;

namespace PhotoSi.UsersService.Database
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDatabase(this IServiceCollection services,IConfiguration configuration)
        {
            services.AddDbContext<UsersDbContext>(opt =>
            {
                opt.UseSqlite(configuration.GetConnectionString("SqlLite"));
            });

            return services;
        }
    }
}
