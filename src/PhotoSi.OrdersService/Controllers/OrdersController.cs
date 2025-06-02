using MediatR;
using Microsoft.AspNetCore.Mvc;
using PhotoSi.OrdersService.Features.CreateOrder;
using PhotoSi.OrdersService.Features.DeleteOrder;
using PhotoSi.OrdersService.Features.GetOrder;
using PhotoSi.OrdersService.Features.ListOrders;
using PhotoSi.OrdersService.Features.UpdateOrder;
using Swashbuckle.AspNetCore.Annotations;

namespace PhotoSi.OrdersService.Controllers
{
    [ApiController]
    [Produces("application/json")]
    public class OrdersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OrdersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [SwaggerOperation(Summary = "Get order by id", Description = "Retrieves the details of a order given its unique id.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Returns the product details", typeof(GetOrderResponse))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Product does not exists")]
        [HttpGet("/orders/{id}", Name = "Get order by id")]
        public async Task<IActionResult> Get(
            CancellationToken cancellationToken,
            [FromRoute] Guid id
        )
        {
            var order = await _mediator.Send(new GetOrderQuery(id), cancellationToken);
            return Ok(order);
        }

        [SwaggerOperation(Summary = "Get list of orders", Description = "Retrieves the list of orders with pagination")]
        [SwaggerResponse(StatusCodes.Status200OK, "Returns the order details", typeof(ListOrdersResponse))]
        [HttpGet("/orders", Name = "List all orders")]
        public async Task<IActionResult> List(
            CancellationToken cancellationToken,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10
        )
        {
            var products = await _mediator.Send(new ListOrdersQuery(pageNumber, pageSize), cancellationToken);
            return Ok(products);
        }


        [SwaggerOperation(Summary = "Create new order", Description = "Creates a new order and returns its unique id.")]
        [SwaggerResponse(StatusCodes.Status201Created, "Order created successfully, returns the new order id", typeof(Guid))]
        [SwaggerResponse(StatusCodes.Status422UnprocessableEntity, "Input data are not accetable")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Order created with a not available user or product")]
        [HttpPost("/orders", Name = "Create order")]
        public async Task<IActionResult> Create(
            CancellationToken cancellationToken,
            [FromBody] CreateOrderCommand command
        )
        {
            var orderId = await _mediator.Send(command, cancellationToken);
            return CreatedAtRoute(
                "Get order by id",
                new { id = orderId },
                new { orderId = orderId }
            );
        }

        [SwaggerOperation(Summary = "Update existing order by id", Description = "Updates existing order lines of an order and returns its unique id.")]
        [SwaggerResponse(StatusCodes.Status202Accepted, "Order updates accepted successfully, returns the order id updated", typeof(Guid))]
        [SwaggerResponse(StatusCodes.Status422UnprocessableEntity, "Input data are not accetable")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Order updated with a not available user or product")]
        [HttpPut("/orders/{id}", Name = "Update order")]
        public async Task<IActionResult> Update(
            CancellationToken cancellationToken,
            [FromRoute] Guid id,
            [FromBody] UpdateOrderCommand command
        )
        {
            var orderId = await _mediator.Send(command with { id = id}, cancellationToken);
            return AcceptedAtRoute(
                "Get order by id",
                new { id = orderId },
                new { orderId = orderId}
            );
        }


        [SwaggerOperation(Summary = "Delete existing order by id", Description = "Deletes an existing order.")]
        [SwaggerResponse(StatusCodes.Status204NoContent, "Order deleted successfully")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Order does not exists")]
        [HttpDelete("/orders/{id}", Name = "Delete order")]
        public async Task<IActionResult> Delete(
            CancellationToken cancellationToken,
            [FromRoute] Guid id
        )
        {
            await _mediator.Send(new DeleteOrderCommand(id), cancellationToken);
            return NoContent();
        }

    }
}
