using Microsoft.EntityFrameworkCore;

namespace PhotoSi.OrdersService.Database
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDatabase(
            this IServiceCollection services, 
            IConfiguration configuration
        )
        {
            services.AddDbContext<OrdersDbContext>(options =>
            {
                options.UseSqlite(configuration.GetConnectionString("SqlLite"));
            });
            return services;
        }
    }
}
