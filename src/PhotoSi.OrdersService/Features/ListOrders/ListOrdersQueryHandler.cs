using MediatR;
using PhotoSi.OrdersService.Features.ListOrders;
using PhotoSi.OrdersService.Repositories;

namespace PhotoSi.ProductsService.Features.ListProducts
{
    public class ListOrdersQueryHandler : IRequestHandler<ListOrdersQuery, ListOrdersResponse>
    {
        private readonly IOrderRepository _orderRespository;

        public ListOrdersQueryHandler(IOrderRepository orderRespository)
        {
            _orderRespository = orderRespository;
        }

        public async Task<ListOrdersResponse> Handle(
            ListOrdersQuery request, 
            CancellationToken cancellationToken
        )
        {
            var totalCount = await _orderRespository
                .CountAsync(cancellationToken);

            var orders = await _orderRespository
                .ListAllAsync(request.pageNumber, request.pageSize, cancellationToken);

            return new ListOrdersResponse
            (
                totalCount,
                request.pageNumber,
                request.pageSize,
                orders.Select(orders => new ListOrderResponse
                (
                    orders.Id,
                    orders.Currency,
                    orders.OrderLines.Sum(ol => ol.Quantity * ol.Product.Price),
                    new ListOrderUserResponse
                    (
                        orders.User.FirstName,
                        orders.User.LastName,
                        new ListOrderUserAddressResponse
                        (
                            orders.User.Address.Street,
                            orders.User.Address.City,
                            orders.User.Address.Country,
                            orders.User.Address.PostalCode
                        )
                    ),
                    orders.OrderLines.Select(ol => new ListOrderLineResponse
                    (
                        ol.Id,
                        ol.Quantity,
                        ol.Notes ?? String.Empty,
                        new ListOrderLineProductResponse
                        (
                            ol.Product.Name,
                            ol.Product.Description,
                            ol.Product.Price,
                            ol.Product.CategoryName
                        )
                    )).ToList()
                )).ToList()
               );

        }
    }
}
