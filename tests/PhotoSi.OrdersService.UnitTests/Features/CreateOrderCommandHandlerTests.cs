using NSubstitute;
using PhotoSi.OrdersService.Features.CreateOrder;
using PhotoSi.OrdersService.Models;
using PhotoSi.OrdersService.Repositories;
using PhotoSi.ProductsService.Features.CreateProduct;
using PhotoSi.Shared.Exceptions;

namespace PhotoSi.OrdersService.UnitTests.Features
{
    public class CreateOrderCommandHandlerTests
    {
        private readonly CreateOrderCommandHandler _handler;
        private readonly IOrderRepository _orderRepository = Substitute.For<IOrderRepository>();
        private readonly IUserRepository _userRepository = Substitute.For<IUserRepository>();
        private readonly IProductRepository _productRepository = Substitute.For<IProductRepository>();

        public CreateOrderCommandHandlerTests()
        {
            _handler = new CreateOrderCommandHandler(_orderRepository, _userRepository, _productRepository);
        }
        private CreateOrderCommand CreateValidCommand(Guid userId, Guid productId, int quantity)
        {
            return new CreateOrderCommand       
            (   userId,
                new List<CreateOrderLineRequest>()
                {
                    new CreateOrderLineRequest(productId, quantity, String.Empty)
                }
            );
        }

        [Fact]
        public async Task Handle_ThrowsBusinessRuleException_WhenUserNotAvailable()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var productId = Guid.NewGuid();
            var request = CreateValidCommand(userId, productId, 5);

            _userRepository.GetByIdAsync(userId, Arg.Any<CancellationToken>())
                .Returns((User?)null);

            // Act & Assert
            await Assert.ThrowsAsync<BusinessRuleException>(() =>
                _handler.Handle(request, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_ThrowsBusinessRuleException_WhenProductNotAvailable()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var productId = Guid.NewGuid();
            var request = CreateValidCommand(userId, productId, 5);

            _userRepository.GetByIdAsync(userId, Arg.Any<CancellationToken>())
                .Returns(new User { Id = userId });

            _productRepository.GetByIdAsync(productId, Arg.Any<CancellationToken>())
                .Returns((Product?)null);

            // Act & Assert
            await Assert.ThrowsAsync<BusinessRuleException>(() =>
                _handler.Handle(request, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_ThrowsException_WhenOrderCreationFails()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var productId = Guid.NewGuid();
            var request = CreateValidCommand(userId, productId, 5);

            _userRepository.GetByIdAsync(userId, Arg.Any<CancellationToken>())
                .Returns(new User { Id = userId });

            _productRepository.GetByIdAsync(productId, Arg.Any<CancellationToken>())
                .Returns(new Product { Id = productId });

            _orderRepository.Create(Arg.Any<Order>()).Returns(false);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() =>
                _handler.Handle(request, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_ReturnsProductId_WhenOrderCreatedSuccessfully()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var productId = Guid.NewGuid();
            var request = CreateValidCommand(userId, productId, 5);

            _userRepository.GetByIdAsync(userId, Arg.Any<CancellationToken>())
                .Returns(new User { Id = userId });

            _productRepository.GetByIdAsync(productId, Arg.Any<CancellationToken>())
                .Returns(new Product { Id = productId });

            _orderRepository.Create(Arg.Any<Order>()).Returns(true);
            _orderRepository.SaveChangesAsync(Arg.Any<CancellationToken>()).Returns(1);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotEqual(Guid.Empty, result);
            await _orderRepository.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Handle_SavesOrder_WhenOrderCreatedSuccessfully()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var productId = Guid.NewGuid();
            var request = CreateValidCommand(userId, productId, 5);

            _userRepository.GetByIdAsync(userId, Arg.Any<CancellationToken>())
                .Returns(new User { Id = userId });

            _productRepository.GetByIdAsync(productId, Arg.Any<CancellationToken>())
                .Returns(new Product { Id = productId });

            _orderRepository.Create(Arg.Any<Order>()).Returns(true);
            _orderRepository.SaveChangesAsync(Arg.Any<CancellationToken>()).Returns(1);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotEqual(Guid.Empty, result);
            await _orderRepository.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
        }
    }
}

