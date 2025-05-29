using MediatR;
using Microsoft.AspNetCore.Mvc;
using PhotoSi.ProductsService.Features.CreateProduct;
using PhotoSi.ProductsService.Features.DeleteProduct;
using PhotoSi.ProductsService.Features.GetProduct;
using PhotoSi.ProductsService.Features.ListProducts;
using PhotoSi.ProductsService.Features.UpdateProduct;

namespace PhotoSi.ProductsService.Controllers
{
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("/products/{id:guid}", Name = "Get one product by id")]
        public async Task<IActionResult> Get(
            CancellationToken cancellationToken,
            [FromRoute] Guid id
        )
        {
            var product = await _mediator.Send(new GetProductQuery(id), cancellationToken);
            return Ok(product);
        }

        [HttpGet("/products", Name = "List all products")]
        public async Task<IActionResult> List(
            CancellationToken cancellationToken,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10
        )
        {
            var products = await _mediator.Send(new ListProductsQuery(pageNumber, pageSize), cancellationToken);
            return Ok(products);
        }


        [HttpPost("/products", Name = "Create new product")]
        public async Task<IActionResult> Create(
            CancellationToken cancellationToken,
            [FromBody] CreateProductCommand command
        )
        {
            var productId = await _mediator.Send(command, cancellationToken);
            return CreatedAtRoute(
                "Get one product by id", 
                new { id = productId }, 
                new { productId = productId }
            );
        }


        [HttpPut("/products/{id:guid}", Name = "Update one product")]
        public async Task<IActionResult> Update(
            CancellationToken cancellationToken,
            [FromRoute] Guid id, 
            [FromBody] UpdateProductCommand command
        )
        {
            var productId = await _mediator.Send(command with { id = id }, cancellationToken);
            return AcceptedAtRoute(
                "Get one product by id", 
                new { id = productId }, 
                new { productId = productId }
            );
        }


        [HttpDelete("/products/{id:guid}", Name = "Delete one product")]
        public async Task<IActionResult> Delete(
            CancellationToken cancellationToken,
            [FromRoute] Guid id)
        {
            await _mediator.Send(new DeleteProductCommand(id), cancellationToken);
            return Accepted();
        }
    }
}
