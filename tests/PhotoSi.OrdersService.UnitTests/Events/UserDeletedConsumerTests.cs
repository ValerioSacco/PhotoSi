using MassTransit;
using Microsoft.Extensions.Logging;
using NSubstitute;
using PhotoSi.OrdersService.Events;
using PhotoSi.OrdersService.Models;
using PhotoSi.OrdersService.Repositories;
using PhotoSi.Shared.Events;

namespace PhotoSi.OrdersService.UnitTests.Events
{
    public class UserDeletedConsumerTests
    {
        private readonly UserDeletedEventConsumer _consumer;
        private readonly IUserRepository _userRepository = Substitute.For<IUserRepository>();
        private readonly ILogger<UserDeletedEventConsumer> _logger = Substitute.For<ILogger<UserDeletedEventConsumer>>();
        private readonly ConsumeContext<UserDeletedEvent> _context = Substitute.For<ConsumeContext<UserDeletedEvent>>();

        public UserDeletedConsumerTests()
        {
            _consumer = new UserDeletedEventConsumer(_userRepository, _logger);
        }

        private static UserDeletedEvent CreateUserDeletedEvent(Guid userId)
        {
            return new UserDeletedEvent(userId);
        }

        [Fact]
        public async Task Consume_RemovesProduct_WhenUserIsReceivedSuccessfully()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var user = new User { Id = userId, IsAvailable = true };
            var userEvent = CreateUserDeletedEvent(userId);
            _context.Message.Returns(userEvent);
            _context.CancellationToken.Returns(CancellationToken.None);

            _userRepository.GetByIdAsync(userId, CancellationToken.None)
                .Returns(user);
            _userRepository.Update(user).Returns(true);

            // Act
            await _consumer.Consume(_context);

            // Assert
            Assert.False(user.IsAvailable);
            await _userRepository.Received(1).SaveChangesAsync(CancellationToken.None);
        }

        [Fact]
        public async Task Consume_LogsWarning_WhenUserNotFound()
        {
            // Arrange
            var userEvent = CreateUserDeletedEvent(Guid.NewGuid());
            _context.Message.Returns(userEvent);
            _context.CancellationToken.Returns(CancellationToken.None);

            _userRepository.GetByIdAsync(userEvent.id, CancellationToken.None)
                .Returns((User?)null);

            // Act
            await _consumer.Consume(_context);

            // Assert
            _logger.Received(1).LogWarning($"User not found: {userEvent.id}");
        }

        [Fact]
        public async Task Consume_LogsInfo_WhenUserIsReceivedSuccessfully()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var user = new User { Id = userId, IsAvailable = true };
            var userEvent = CreateUserDeletedEvent(userId);
            _context.Message.Returns(userEvent);
            _context.CancellationToken.Returns(CancellationToken.None);

            _userRepository.GetByIdAsync(userId, CancellationToken.None)
                .Returns(user);
            _userRepository.Update(user).Returns(true);

            // Act
            await _consumer.Consume(_context);

            // Assert
            Assert.False(user.IsAvailable);
            _logger.Received(1).LogInformation($"User set as unavailable successfully: {user.Id}");
        }

        [Fact]
        public async Task Consume_LogsError_WhenUserDeletionFails()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var user = new User { Id = userId, IsAvailable = true };
            var userEvent = CreateUserDeletedEvent(userId);
            _context.Message.Returns(userEvent);
            _context.CancellationToken.Returns(CancellationToken.None);

            _userRepository.GetByIdAsync(userId, CancellationToken.None)
                .Returns(user);
            _userRepository.Update(user).Returns(false);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<Exception>(() => _consumer.Consume(_context));
            Assert.Equal("Failed to set user unavailable in the database.", ex.Message);
            _logger.Received(1).LogError($"Failed to set user as unavailable: {user.Id}");
        }
    }
}
