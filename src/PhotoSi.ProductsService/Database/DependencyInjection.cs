using Microsoft.EntityFrameworkCore;

namespace PhotoSi.ProductsService.Database
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDatabase(
            this IServiceCollection services, 
            IConfiguration configuration
        )
        {
            services.AddDbContext<ProductsDbContext>(opt =>
            {
                opt.UseSqlite(configuration.GetConnectionString("SqlLite"));
            });

            return services;
        }
    }
}
