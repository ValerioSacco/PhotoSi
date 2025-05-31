using NSubstitute;
using PhotoSi.OrdersService.Features.UpdateOrder;
using PhotoSi.OrdersService.Models;
using PhotoSi.OrdersService.Repositories;
using PhotoSi.ProductsService.Features.UpdateProduct;
using PhotoSi.Shared.Exceptions;

namespace PhotoSi.OrdersService.UnitTests.Features
{
    public class UpdateOrderCommandHandlerTests
    {
        private readonly UpdateOrderCommandHandler _handler;
        private readonly IOrderRepository _orderRepository = Substitute.For<IOrderRepository>();
        private readonly IProductRepository _productRepository = Substitute.For<IProductRepository>();

        public UpdateOrderCommandHandlerTests()
        {
            _handler = new UpdateOrderCommandHandler(_orderRepository, _productRepository);
        }

        private UpdateOrderCommand CreateValidCommand(Guid id)
        {
            return new UpdateOrderCommand(
                id,
                new List<UpdateOrderLineRequest>
                {
                    new UpdateOrderLineRequest(
                        Guid.NewGuid(),
                        Guid.NewGuid(),
                        2,
                        "test"
                    )
                });
        }

        private Order CreateOrder(Guid orderId, List<OrderLine> orderLines)
        {
            return new Order
            {
                Id = orderId,
                OrderLines = orderLines
            };
        }

        [Fact]
        public async Task Handle_OrderNotFound_ThrowsNotFoundException()
        {
            // Arrange
            var command = CreateValidCommand(Guid.NewGuid());
            _orderRepository.GetByIdAsync(command.id, Arg.Any<CancellationToken>())
                .Returns((Order?)null);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() =>
                _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_OrderLineNotFound_ThrowsNotFoundException()
        {
            // Arrange
            var command = CreateValidCommand(Guid.NewGuid());
            var order = CreateOrder(command.id, new List<OrderLine>()); // No order lines
            _orderRepository.GetByIdAsync(command.id, Arg.Any<CancellationToken>())
                .Returns(order);

            _productRepository.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
                .Returns(new Product { Id = command.orderLines[0].productId });

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() =>
                _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_ProductNotFound_ThrowsBusinessRuleException()
        {
            // Arrange
            var command = CreateValidCommand(Guid.NewGuid());
            var orderLine = new OrderLine
            {
                Id = command.orderLines[0].orderLineId,
                ProductId = Guid.NewGuid(),
                Quantity = 1,
                Notes = "test"
            };
            var order = CreateOrder(command.id, new List<OrderLine> { orderLine });
            _orderRepository.GetByIdAsync(command.id, Arg.Any<CancellationToken>())
                .Returns(order);

            _productRepository.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
                .Returns((Product?)null);

            // Act & Assert
            await Assert.ThrowsAsync<BusinessRuleException>(() =>
                _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_UpdateFails_ThrowsException()
        {
            // Arrange
            var command = CreateValidCommand(Guid.NewGuid());
            var orderLine = new OrderLine
            {
                Id = command.orderLines[0].orderLineId,
                ProductId = command.orderLines[0].productId,
                Quantity = 1,
                Notes = "test"
            };
            var order = CreateOrder(command.id, new List<OrderLine> { orderLine });
            _orderRepository.GetByIdAsync(command.id, Arg.Any<CancellationToken>())
                .Returns(order);

            _productRepository.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
                .Returns(new Product { Id = command.orderLines[0].productId });

            _orderRepository.Update(order).Returns(false);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() =>
                _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_ValidRequest_UpdatesOrderAndReturnsId()
        {
            // Arrange
            var command = CreateValidCommand(Guid.NewGuid());
            var orderLine = new OrderLine
            {
                Id = command.orderLines[0].orderLineId,
                ProductId = command.orderLines[0].productId,
                Quantity = 1,
                Notes = "old"
            };
            var order = CreateOrder(command.id, new List<OrderLine> { orderLine });
            _orderRepository.GetByIdAsync(command.id, Arg.Any<CancellationToken>())
                .Returns(order);

            _productRepository.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
                .Returns(new Product { Id = command.orderLines[0].productId });

            _orderRepository.Update(order).Returns(true);
            _orderRepository.SaveChangesAsync(Arg.Any<CancellationToken>()).Returns(1);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(order.Id, result);
            Assert.Equal(command.orderLines[0].productId, order.OrderLines.First().ProductId);
            Assert.Equal(command.orderLines[0].quantity, order.OrderLines.First().Quantity);
            Assert.Equal(command.orderLines[0].notes, order.OrderLines.First().Notes);
            await _orderRepository.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
        }

    }
}