using Microsoft.EntityFrameworkCore;
using PhotoSi.AddressBookService.Models;

namespace PhotoSi.AddressBookService.Database
{
    public class AddressBookDbContext : DbContext
    {
        public AddressBookDbContext(DbContextOptions<AddressBookDbContext> options) 
           : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AddressBookDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Address> Addresses { get; set; }
    }
}
