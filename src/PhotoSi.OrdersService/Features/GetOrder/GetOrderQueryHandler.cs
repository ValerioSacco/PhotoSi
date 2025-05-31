using MediatR;
using PhotoSi.OrdersService.Repositories;
using PhotoSi.Shared.Exceptions;

namespace PhotoSi.OrdersService.Features.GetOrder
{
    public class GetOrderQueryHandler : IRequestHandler<GetOrderQuery, GetOrderResponse>
    {
        private readonly IOrderRepository _orderRespository;

        public GetOrderQueryHandler(IOrderRepository orderRespository)
        {
            _orderRespository = orderRespository;
        }

        public async Task<GetOrderResponse> Handle(
            GetOrderQuery request, 
            CancellationToken cancellationToken
        )
        {
            var order = await _orderRespository
                .GetByIdAsync(request.id, cancellationToken);

            if (order is null) {
                throw new NotFoundException("The order requested does not exist.");
            }

            return new GetOrderResponse
            (
                order.Id,
                order.Currency,
                order.OrderLines.Sum(ol => ol.Quantity * ol.Product.Price),
                new GetOrderUserResponse(
                    order.User.FirstName,
                    order.User.LastName,
                    new GetOrderUserAddressResponse
                    (
                        order.User.Address.Street,
                        order.User.Address.City,
                        order.User.Address.Country,
                        order.User.Address.PostalCode
                    )
                ),
                order.OrderLines.Select(ol => new GetOrderLineResponse
                (
                    ol.Id,
                    ol.Quantity,
                    ol.Notes ?? String.Empty,
                    new GetOrderLineProductResponse
                    (
                        ol.Product.Name,
                        ol.Product.Description,
                        ol.Product.Price,
                        ol.Product.CategoryName
                    )
                )).ToList()
            );
        }
    }
}
