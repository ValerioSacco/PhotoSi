using MediatR;

namespace PhotoSi.OrdersService.Features.UpdateOrder
{
   public record UpdateOrderLineRequest(
       Guid orderLineId,
       Guid productId,
       int quantity,
       string? notes
   );

    public record UpdateOrderCommand(
       Guid id, 
       IList<UpdateOrderLineRequest> orderLines
   ) : IRequest<Guid>;
}
