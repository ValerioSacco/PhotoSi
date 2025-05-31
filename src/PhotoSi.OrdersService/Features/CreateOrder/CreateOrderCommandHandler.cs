using MediatR;
using PhotoSi.OrdersService.Features.CreateOrder;
using PhotoSi.OrdersService.Models;
using PhotoSi.OrdersService.Repositories;
using PhotoSi.Shared.Exceptions;

namespace PhotoSi.ProductsService.Features.CreateProduct
{
    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, Guid>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IUserRepository _userRepository;
        private readonly IProductRepository _productRepository;

        public CreateOrderCommandHandler(
            IOrderRepository orderRepository,
            IUserRepository userRepository,
            IProductRepository productRepository)
        {
            _orderRepository = orderRepository;
            _userRepository = userRepository;
            _productRepository = productRepository;
        }

        public async Task<Guid> Handle(
            CreateOrderCommand request,
            CancellationToken cancellationToken
        )
        {
            User user = await ValidateUser(request, cancellationToken);
            List<OrderLine> orderLines = await ValidateOrderLines(request, cancellationToken);

            var order = new Order()
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                OrderLines = orderLines
            };

            if (_orderRepository.Create(order))
            {
                await _orderRepository.SaveChangesAsync(cancellationToken);
                return order.Id;
            }
            else
            {
                throw new Exception("Failed to create order");
            }
        }

        private async Task<User> ValidateUser(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.userId, cancellationToken);
            if (user is null)
            {
                throw new BusinessRuleException("An order must be associated to an available user.");
            }

            return user;
        }

        private async Task<List<OrderLine>> ValidateOrderLines(CreateOrderCommand request, CancellationToken cancellationToken)
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
