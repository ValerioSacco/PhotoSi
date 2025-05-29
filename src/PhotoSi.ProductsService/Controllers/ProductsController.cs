using MediatR;
using Microsoft.AspNetCore.Mvc;
using PhotoSi.ProductsService.Features.CreateProduct;
using PhotoSi.ProductsService.Features.GetProduct;
using PhotoSi.ProductsService.Features.ListProducts;

namespace PhotoSi.ProductsService.Controllers
{
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ILogger<ProductsController> _logger;
        private readonly IMediator _mediator;

        public ProductsController(IMediator mediator, ILogger<ProductsController> logger)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet("/products/{id:guid}", Name = "Get one product by id")]
        public async Task<IActionResult> Get(
            [FromRoute] Guid id, 
            CancellationToken cancellationToken
        )
        {
            var product = await _mediator.Send(new GetProductQuery(id), cancellationToken);
            return Ok(product);
        }

        [HttpGet("/products", Name = "List all products")]
        public async Task<IActionResult> List(
            [FromQuery] int pageNumber,
            [FromQuery] int pageSize,
            CancellationToken cancellationToken)
        {
            var products = await _mediator.Send(new ListProductsQuery(pageNumber, pageSize), cancellationToken);
            return Ok(products);
        }


        [HttpPost("/products", Name = "Create new product")]
        public async Task<IActionResult> Create(
            [FromBody] CreateProductCommand command, 
            CancellationToken cancellationToken
        )
        {
            var productId = await _mediator.Send(command, cancellationToken);
            return CreatedAtRoute(
                "Get one product by id", 
                new { id = productId }, 
                new { productId = productId }
            );
        }
    }
}
