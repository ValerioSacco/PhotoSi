using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PhotoSi.AddressBookService.Database;
using PhotoSi.AddressBookService.Models;

namespace PhotoSi.AddressBookService.Controllers
{
    [ApiController]
    public class AddressBookController : ControllerBase
    {
        private readonly AddressBookDbContext _dbContext;

        public AddressBookController(AddressBookDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("/addresses/{country}/{city}/{postalCode}/{street}", Name = "Find address on address book by postal code and street")]
        public async Task<IActionResult> Get(
            CancellationToken cancellationToken,
            string country,
            string city,
            string postalCode,
            string street
        )
        {
            var address = await _dbContext.Addresses
                .FirstOrDefaultAsync(
                    a => a.Country == country &&
                    a.City == city &&
                    a.PostalCode == postalCode &&
                    a.Street == street,
                    cancellationToken
                );

            if (address is null)
            {
                return NotFound();
            }

            return Ok(address);
        }


        [HttpGet("/addresses", Name = "List all addresses in address book")]
        public async Task<IActionResult> List(
            CancellationToken cancellationToken,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10
        )
        {
            var totalCount = await _dbContext.Addresses
                .CountAsync(cancellationToken);

            var addresses = await _dbContext.Addresses
                .AsNoTracking()
                .OrderBy(a => a.Country).ThenBy(a => a.City)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return Ok(new ListAddressesResponse(
                totalCount,
                pageNumber,
                pageSize,
                addresses
            ));
        }

    }

    public record ListAddressesResponse(int totalCount, int pageNumber, int pageSize, IList<Address> addresses);

}
