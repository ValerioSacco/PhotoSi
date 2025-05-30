using Microsoft.EntityFrameworkCore;

namespace PhotoSi.AddressBookService.Database
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AddressBookDbContext>(opt =>
            {
                opt.UseSqlite(configuration.GetConnectionString("SqlLite"));
            });
            return services;
        }
    }
}
