using MediatR;
using Microsoft.AspNetCore.Mvc;
using PhotoSi.OrdersService.Features.CreateOrder;
using PhotoSi.OrdersService.Features.DeleteOrder;
using PhotoSi.OrdersService.Features.GetOrder;
using PhotoSi.OrdersService.Features.ListOrders;
using PhotoSi.OrdersService.Features.UpdateOrder;

namespace PhotoSi.OrdersService.Controllers
{
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OrdersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("/orders/{id}", Name = "Get order by id")]
        public async Task<IActionResult> Get(
            CancellationToken cancellationToken,
            [FromRoute] Guid id
        )
        {
            var order = await _mediator.Send(new GetOrderQuery(id), cancellationToken);
            return Ok(order);
        }


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
