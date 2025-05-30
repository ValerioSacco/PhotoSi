using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PhotoSi.AddressBookService.Database;
using System.Runtime.InteropServices;

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

            if(address is null)
            {
                return NotFound();
            }

            return Ok(address);
        }
    }
}
