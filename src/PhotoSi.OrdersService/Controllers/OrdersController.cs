using MediatR;
using Microsoft.AspNetCore.Mvc;
using PhotoSi.OrdersService.Features.CreateOrder;
using PhotoSi.OrdersService.Features.GetOrder;
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

    }
}
