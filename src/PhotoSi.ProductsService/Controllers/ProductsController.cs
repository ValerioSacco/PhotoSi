using MediatR;
using Microsoft.AspNetCore.Mvc;
using PhotoSi.ProductsService.Features.CreateProduct;
using PhotoSi.ProductsService.Features.GetProduct;

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

        [HttpGet("/products/{id:guid}", Name = "Get one product by code")]
        public async Task<IActionResult> Get(
            [FromRoute] Guid id, 
            CancellationToken cancellationToken
        )
        {
            var result = await _mediator.Send(new GetProductQuery(id), cancellationToken);
            return Ok(result);
        }

        [HttpPost("/products", Name = "Create new product")]
        public async Task<IActionResult> Create(
            [FromBody] CreateProductCommand command, 
            CancellationToken cancellationToken
        )
        {
            var result = await _mediator.Send(command, cancellationToken);
            return Created();
        }
    }
}
