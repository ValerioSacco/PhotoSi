using MediatR;
using Microsoft.AspNetCore.Mvc;
using PhotoSi.OrdersService.Features.CreateOrder;
using PhotoSi.OrdersService.Features.GetOrder;

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
                new { orderId = orderId}
            );
        }
    }
}
