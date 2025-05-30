using Microsoft.EntityFrameworkCore;

namespace PhotoSi.AddressBookService.Database
{
    public static class MigrationExtensions
    {
        public static void ApplyMigrations(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AddressBookDbContext>();
                dbContext.Database.Migrate();
            }
        }
    }
}
