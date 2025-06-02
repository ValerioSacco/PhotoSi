using MediatR;
using Microsoft.AspNetCore.Mvc;
using PhotoSi.ProductsService.Features.CreateProduct;
using PhotoSi.ProductsService.Features.DeleteProduct;
using PhotoSi.ProductsService.Features.GetProduct;
using PhotoSi.ProductsService.Features.ListProducts;
using PhotoSi.ProductsService.Features.UpdateProduct;
using Swashbuckle.AspNetCore.Annotations;

namespace PhotoSi.ProductsService.Controllers
{
    [ApiController]
    [Produces("application/json")]
    public class ProductsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [SwaggerOperation(Summary = "Get product by id", Description = "Retrieves the details of a product given its unique id.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Returns the product details", typeof(GetProductResponse))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Product does not exists")]
        [HttpGet("/products/{id:guid}", Name = "Get one product by id")]
        public async Task<IActionResult> Get(
            CancellationToken cancellationToken,
            [FromRoute] Guid id
        )
        {
            var product = await _mediator.Send(new GetProductQuery(id), cancellationToken);
            return Ok(product);
        }


        [SwaggerOperation(Summary = "Get list of products", Description = "Retrieves the list of products with pagination")]
        [SwaggerResponse(StatusCodes.Status200OK, "Returns the product details", typeof(ListProductsResponse))]
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

        [SwaggerOperation(Summary = "Create new product", Description = "Creates a new product and returns its unique id.")]
        [SwaggerResponse(StatusCodes.Status201Created, "Product created successfully, returns the new product id", typeof(Guid))]
        [SwaggerResponse(StatusCodes.Status422UnprocessableEntity, "Input data are not accetable")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Product created with a not existing category")]
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

        [SwaggerOperation(Summary = "Update existing product by id", Description = "Updates an existing product and returns its unique id.")]
        [SwaggerResponse(StatusCodes.Status202Accepted, "Product updates accepted successfully, returns the product id updated", typeof(Guid))]
        [SwaggerResponse(StatusCodes.Status422UnprocessableEntity, "Input data are not accetable")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Product updated with a not existing category")]
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


        [SwaggerOperation(Summary = "Delete existing product by id", Description = "Deletes an existing product.")]
        [SwaggerResponse(StatusCodes.Status204NoContent, "Product deleted successfully")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Product does not exists")]
        [HttpDelete("/products/{id:guid}", Name = "Delete one product")]
        public async Task<IActionResult> Delete(
            CancellationToken cancellationToken,
            [FromRoute] Guid id)
        {
            await _mediator.Send(new DeleteProductCommand(id), cancellationToken);
            return NoContent();
        }
    }
}
