using MediatR;
using PhotoSi.OrdersService.Features.UpdateOrder;
using PhotoSi.OrdersService.Models;
using PhotoSi.OrdersService.Repositories;
using PhotoSi.Shared.Exceptions;

namespace PhotoSi.ProductsService.Features.UpdateProduct
{
    public class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand, Guid>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;

        public UpdateOrderCommandHandler(
            IOrderRepository orderRepository,
            IProductRepository productRepository)
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
        }
        public async Task<Guid> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
        {
            var order = await _orderRepository.GetByIdAsync(request.id, cancellationToken);
            if (order is null)
            {
                throw new NotFoundException($"Order with id {request.id} does not exist.");
            }

            List<OrderLine> orderLines = await ValidateOrderLines(request, cancellationToken);


            foreach (var orderLine in request.orderLines)
            {
                var lineToUpdate = order
                    .OrderLines
                    .FirstOrDefault(ol => ol.Id == orderLine.orderLineId);

                if (lineToUpdate is null)
                {
                    throw new NotFoundException($"Order line with id {orderLine.orderLineId} does not exist in order {request.id}.");
                }

                lineToUpdate.ProductId = orderLine.productId;
                lineToUpdate.Quantity = orderLine.quantity;
                lineToUpdate.Notes = orderLine.notes;
            }
                

            if (_orderRepository.Update(order))
            {
                await _orderRepository.SaveChangesAsync(cancellationToken);
                return order.Id;
            }
            else
            {
                throw new Exception("Failed to update order");
            }
        }

        private async Task<List<OrderLine>> ValidateOrderLines(
            UpdateOrderCommand request, 
            CancellationToken cancellationToken
        )
        {
            var orderLines = new List<OrderLine>();
            foreach (var orderLine in request.orderLines)
            {
                var product = await _productRepository.GetByIdAsync(orderLine.productId, cancellationToken);
                if (product is null)
                {
                    throw new BusinessRuleException($"Product with id {orderLine.productId} is not available.");
                }
                orderLines.Add(new OrderLine
                {
                    ProductId = product.Id,
                    Quantity = orderLine.quantity
                });
            }

            return orderLines;
        }
    }
}
