using MassTransit;
using Microsoft.Extensions.Logging;
using NSubstitute;
using PhotoSi.OrdersService.Events;
using PhotoSi.OrdersService.Models;
using PhotoSi.OrdersService.Repositories;
using PhotoSi.Shared.Events;


namespace PhotoSi.OrdersService.UnitTests.Events
{
    public class UserUpdatedConsumerTests
    {
        private readonly UserUpdatedEventConsumer _consumer;
        private readonly IUserRepository _userRepository = Substitute.For<IUserRepository>();
        private readonly ILogger<UserUpdatedEventConsumer> _logger = Substitute.For<ILogger<UserUpdatedEventConsumer>>();
        private readonly ConsumeContext<UserUpdatedEvent> _context = Substitute.For<ConsumeContext<UserUpdatedEvent>>();

        public UserUpdatedConsumerTests()
        {
            _consumer = new UserUpdatedEventConsumer(_userRepository, _logger);
        }

        private UserUpdatedEvent CreateUserUpdatedEvent(Guid id)
        {
            return new UserUpdatedEvent
            (
                id,
                "Test FirstName",
                "Test LastName",
                "Test Country",
                "Test City",
                "Test Street",
                "Test PostalCode"
            );
        }

        [Fact]
        public async Task Consume_LogsWarning_WhenUserNotFound()
        {
            // Arrange
            var userEvent = CreateUserUpdatedEvent(Guid.NewGuid());
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
        public async Task Consume_SavesProduct_WhenUserIsReceivedSuccessfully()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var user = new User
            {
                Id = userId,
                FirstName = "OldFirstName",
                LastName = "OldLastName",
                Address = new ShipmentAddress
                {
                    Country = "OldCountry",
                    City = "OldCity",
                    Street = "OldStreet",
                    PostalCode = "OldPostalCode"
                }

            };
            var userEvent = CreateUserUpdatedEvent(userId);
            _context.Message.Returns(userEvent);
            _context.CancellationToken.Returns(CancellationToken.None);
            _userRepository.GetByIdAsync(userId, CancellationToken.None)
                .Returns(user);
            _userRepository.Update(user).Returns(true);

            // Act
            await _consumer.Consume(_context);

            // Assert
            _userRepository.Received(1).Update(user);
            await _userRepository.Received(1).SaveChangesAsync(CancellationToken.None);
        }


        [Fact]
        public async Task Consume_LogsInfo_WhenUserIsReceivedSuccessfully()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var user = new User
            {
                Id = userId,
                FirstName = "OldFirstName",
                LastName = "OldLastName",
                Address = new ShipmentAddress
                {
                    Country = "OldCountry",
                    City = "OldCity",
                    Street = "OldStreet",
                    PostalCode = "OldPostalCode"
                }

            };
            var userEvent = CreateUserUpdatedEvent(userId);
            _context.Message.Returns(userEvent);
            _context.CancellationToken.Returns(CancellationToken.None);
            _userRepository.GetByIdAsync(userId, CancellationToken.None)
                .Returns(user);
            _userRepository.Update(user).Returns(true);

            // Act
            await _consumer.Consume(_context);

            // Assert
            _logger.Received(1).LogInformation($"User updated successfully: {userId}");
        }

        [Fact]
        public async Task Consume_LogsError_WhenUserUpdateFails()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var user = new User
            {
                Id = userId,
                FirstName = "OldFirstName",
                LastName = "OldLastName",
                Address = new ShipmentAddress
                {
                    Country = "OldCountry",
                    City = "OldCity",
                    Street = "OldStreet",
                    PostalCode = "OldPostalCode"
                }

            };
            var userEvent = CreateUserUpdatedEvent(userId);
            _context.Message.Returns(userEvent);
            _context.CancellationToken.Returns(CancellationToken.None);
            _userRepository.GetByIdAsync(userId, CancellationToken.None)
                .Returns(user);
            _userRepository.Update(user).Returns(false);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<Exception>(() => _consumer.Consume(_context));
            Assert.Equal("Failed to update user in the database.", ex.Message);
            _logger.Received(1).LogError($"Failed to update user: {userId}");
        }
    }
}

