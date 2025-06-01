using Microsoft.EntityFrameworkCore;
using PhotoSi.AddressBookService.Database;
using PhotoSi.AddressBookService.Models;

namespace PhotoSi.AddressBookService.UnitTests.Controllers
{
    public class AddressBookDbFixture : IDisposable
    {
        public AddressBookDbContext DbContext { get; }
        public AddressBookDbFixture()
        {
            var options = new DbContextOptionsBuilder<AddressBookDbContext>()
                .UseInMemoryDatabase("TestDb")
                .Options;

            DbContext = new AddressBookDbContext(options);
            if (!DbContext.Addresses.Any())
            {
                DbContext.Addresses.AddRange(
                    new Address { Country = "Italia", City = "Milano", PostalCode = "20100", Street = "Via Roma" },
                    new Address { Country = "Italia", City = "Roma", PostalCode = "00100", Street = "Via Milano" },
                    new Address { Country = "Francia", City = "Paris", PostalCode = "75000", Street = "Rue de Rivoli" }
                );
                DbContext.SaveChanges();
            }
        }

        public void Dispose()
        {
            DbContext.Dispose();
        }
    }
}
