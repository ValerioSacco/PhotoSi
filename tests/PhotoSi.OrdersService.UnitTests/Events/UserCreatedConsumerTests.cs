using MassTransit;
using Microsoft.Extensions.Logging;
using NSubstitute;
using PhotoSi.OrdersService.Events;
using PhotoSi.OrdersService.Models;
using PhotoSi.OrdersService.Repositories;
using PhotoSi.Shared.Events;

namespace PhotoSi.OrdersService.UnitTests.Events
{
    public class UserCreatedConsumerTests
    {
        private readonly UserCreatedEventConsumer _consumer;
        private readonly IUserRepository _userRepository = Substitute.For<IUserRepository>();
        private readonly ILogger<UserCreatedEventConsumer> _logger = Substitute.For<ILogger<UserCreatedEventConsumer>>();
        private readonly ConsumeContext<UserCreatedEvent> _context = Substitute.For<ConsumeContext<UserCreatedEvent>>();

        public UserCreatedConsumerTests()
        {
            _consumer = new UserCreatedEventConsumer(_userRepository, _logger);
        }

        private UserCreatedEvent UserCreatedEvent()
        {
            return new UserCreatedEvent(
                Guid.NewGuid(),
                "Mario",
                "Rossi",
                "Italia",
                "Milano",
                "Via Roma 1",
                "12345"
            );
        }

        [Fact]
        public async Task Consume_SavesUser_WhenUserIsReceivedSuccessfully()
        {
            // Arrange
            var userEvent = UserCreatedEvent();
            _context.Message.Returns(userEvent);
            _context.CancellationToken.Returns(CancellationToken.None);

            _userRepository.Create(Arg.Any<User>()).Returns(true);
            _userRepository.SaveChangesAsync(Arg.Any<CancellationToken>()).Returns(Task.FromResult(1));

            // Act
            await _consumer.Consume(_context);

            // Assert
            _userRepository.Received(1).Create(Arg.Is<User>(u =>
                u.Id == userEvent.id &&
                u.FirstName == userEvent.firstname &&
                u.LastName == userEvent.lastname &&
                u.Address.Country == userEvent.country &&
                u.Address.City == userEvent.city &&
                u.Address.Street == userEvent.street &&
                u.Address.PostalCode == userEvent.postalCode
            ));
            await _userRepository.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
        }


        [Fact]
        public async Task Consume_LogsInfo_WhenUserIsReceivedSuccessfully()
        {
            // Arrange
            var userEvent = UserCreatedEvent();
            _context.Message.Returns(userEvent);
            _context.CancellationToken.Returns(CancellationToken.None);

            _userRepository.Create(Arg.Any<User>()).Returns(true);
            _userRepository.SaveChangesAsync(Arg.Any<CancellationToken>()).Returns(Task.FromResult(1));

            // Act
            await _consumer.Consume(_context);

            // Assert
            _userRepository.Received(1).Create(Arg.Is<User>(u =>
                u.Id == userEvent.id &&
                u.FirstName == userEvent.firstname &&
                u.LastName == userEvent.lastname &&
                u.Address.Country == userEvent.country &&
                u.Address.City == userEvent.city &&
                u.Address.Street == userEvent.street &&
                u.Address.PostalCode == userEvent.postalCode
            ));
            _logger.Received(1).LogInformation($"User created successfully: {userEvent.id}");
        }


        [Fact]
        public async Task Consume_LogsError_WhenUserCreationFails()
        {
            // Arrange
            var userEvent = UserCreatedEvent();
            _context.Message.Returns(userEvent);
            _context.CancellationToken.Returns(CancellationToken.None);

            _userRepository.Create(Arg.Any<User>()).Returns(false);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<Exception>(() => _consumer.Consume(_context));
            _logger.Received(1).LogError($"Failed to create user: {userEvent.id}");
        }
    }
}
