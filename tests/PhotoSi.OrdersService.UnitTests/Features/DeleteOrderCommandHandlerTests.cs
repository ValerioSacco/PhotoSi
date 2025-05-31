using NSubstitute;
using PhotoSi.OrdersService.Features.DeleteOrder;
using PhotoSi.OrdersService.Models;
using PhotoSi.OrdersService.Repositories;
using PhotoSi.Shared.Exceptions;


namespace PhotoSi.OrdersService.UnitTests.Features
{
    public class DeleteOrderCommandHandlerTests
    {
        private readonly IOrderRepository _orderRepository = Substitute.For<IOrderRepository>();
        private readonly DeleteOrderCommandHandler _handler;

        public DeleteOrderCommandHandlerTests()
        {
            _handler = new DeleteOrderCommandHandler(_orderRepository);
        }

        [Fact]
        public async Task Handle_OrderNotFound_ThrowsNotFoundException()
        {
            // Arrange
            var command = new DeleteOrderCommand(Guid.NewGuid());
            _orderRepository.GetByIdAsync(command.id, Arg.Any<CancellationToken>())
                .Returns((Order?)null);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() =>
                _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_DeleteFails_ThrowsException()
        {
            // Arrange
            var command = new DeleteOrderCommand(Guid.NewGuid());
            var order = new Order { Id = command.id };
            _orderRepository.GetByIdAsync(command.id, Arg.Any<CancellationToken>())
                .Returns(order);
            _orderRepository.Delete(order).Returns(false);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() =>
                _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_ValidOrder_DeletesAndSaves()
        {
            // Arrange
            var command = new DeleteOrderCommand(Guid.NewGuid());
            var order = new Order { Id = command.id };
            _orderRepository.GetByIdAsync(command.id, Arg.Any<CancellationToken>())
                .Returns(order);
            _orderRepository.Delete(order).Returns(true);
            _orderRepository.SaveChangesAsync(Arg.Any<CancellationToken>()).Returns(1);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            await _orderRepository.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
        }
    }
}
