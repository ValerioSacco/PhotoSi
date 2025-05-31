
using MediatR;

namespace PhotoSi.OrdersService.Features.CreateOrder
{
    public record CreateOrderLineRequest(
        Guid productId,
        int quantity,
        string? notes
    );

    public record CreateOrderCommand(
        Guid userId,
        IList<CreateOrderLineRequest> orderLines
    ) : IRequest<Guid>;

}
