using Microsoft.EntityFrameworkCore;

namespace PhotoSi.OrdersService.Database
{
    public static class MigrationExtensions
    {
        public static void ApplyMigrations(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<OrdersDbContext>();
                dbContext.Database.Migrate();
            }
        }
    }
}
