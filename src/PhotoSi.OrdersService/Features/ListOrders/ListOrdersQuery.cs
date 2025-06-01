using MediatR;

namespace PhotoSi.OrdersService.Features.ListOrders
{
    public record ListOrdersQuery(int pageNumber, int pageSize) : IRequest<ListOrdersResponse>;
}
