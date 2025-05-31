using MediatR;

namespace PhotoSi.OrdersService.Features.DeleteOrder
{
    public record DeleteOrderCommand(Guid id) : IRequest;
}
