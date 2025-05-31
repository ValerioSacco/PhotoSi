using MediatR;

namespace PhotoSi.OrdersService.Features.GetOrder
{
    public record GetOrderQuery(Guid id) : IRequest<GetOrderResponse>;
}
  
