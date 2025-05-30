using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PhotoSi.AddressBookService.Database;

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

        [HttpGet("/addresses/{postalCode}/{street}", Name = "Find address on address book by postal code and street")]
        public async Task<IActionResult> Get(
            CancellationToken cancellationToken, 
            string postalCode, 
            string street
        )
        {
            var address = await _dbContext.Addresses
                .FirstOrDefaultAsync(
                    a => a.PostalCode == postalCode && a.Street == street,
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
