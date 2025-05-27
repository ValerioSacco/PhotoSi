using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PhotoSi.ProductsService.Database;

namespace PhotoSi.ProductsService.Controllers
{
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ILogger<ProductsController> _logger;
        private readonly ProductsDbContext _dbContext;

        public ProductsController(ProductsDbContext dbContext, ILogger<ProductsController> logger)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        [HttpGet("/products", Name = "List all products")]
        public async Task<IActionResult> Get()
        {
            var products = await _dbContext
                .Products
                .Include(p => p.Category)
                .ToListAsync();
            return Ok(products);
        }
    }
}
