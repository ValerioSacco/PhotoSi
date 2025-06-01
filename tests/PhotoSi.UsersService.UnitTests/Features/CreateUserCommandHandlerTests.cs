using MassTransit;
using NSubstitute;
using PhotoSi.Shared.Events;
using PhotoSi.Shared.Exceptions;
using PhotoSi.UsersService.Features.CreateUser;
using PhotoSi.UsersService.Features.UpdateUser;
using PhotoSi.UsersService.Models;
using PhotoSi.UsersService.Repositories;
using PhotoSi.UsersService.Services;

namespace PhotoSi.UsersService.UnitTests.Features
{
    public class CreateUserCommandHandlerTests
    {
        private readonly UpdateUserCommandHandler _handler;
        private readonly IUserRepository _userRepository = Substitute.For<IUserRepository>();
        private readonly IAddressChecker _addressChecker = Substitute.For<IAddressChecker>();
        private readonly IPublishEndpoint _publishEndpoint = Substitute.For<IPublishEndpoint>();

        public CreateUserCommandHandlerTests()
        {
            _handler = new CreateUserCommandHandler(
                _userRepository,
                _addressChecker,
                _publishEndpoint
            );
        }

        private UpdateUserCommand CreateValidCommand()
        {
            return new UpdateUserCommand
            (
                "testuser",
                "Test",
                "User",
                "test@example.com",
                "1234567890",
                "http://example.com/pic.jpg",
                new ShipmentAddressRequest
                (
                    "Country",
                    "City",
                    "12345",
                    "Street 1"
                )
            );
        }


        [Fact]
        public async Task Handle_ThrowsBusinessRuleException_WhenAddressIsInvalid()
        {
            // Arrange
            var command = CreateValidCommand();
            _addressChecker.IsAddressValidAsync(Arg.Any<ShipmentAddress>(), Arg.Any<CancellationToken>())
                .Returns(false);

            // Act & Assert
            await Assert.ThrowsAsync<BusinessRuleException>(() =>
                _handler.Handle(command, CancellationToken.None));
            _userRepository.DidNotReceive().Create(Arg.Any<User>());
            await _publishEndpoint.DidNotReceive().Publish(Arg.Any<UserCreatedEvent>(), Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Handle_ThrowsException_WhenUserCreationFails()
        {
            // Arrange
            var command = CreateValidCommand();
            _addressChecker.IsAddressValidAsync(Arg.Any<ShipmentAddress>(), Arg.Any<CancellationToken>())
                .Returns(true);
            _userRepository.Create(Arg.Any<User>()).Returns(false);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() =>
                _handler.Handle(command, CancellationToken.None));
            await _publishEndpoint.DidNotReceive().Publish(Arg.Any<UserCreatedEvent>(), Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Handle_ReturnsProductId_WhenUserCreatedSuccessfully()
        {
            // Arrange
            var command = CreateValidCommand();
            _addressChecker.IsAddressValidAsync(Arg.Any<ShipmentAddress>(), Arg.Any<CancellationToken>())
                .Returns(true);
            _userRepository.Create(Arg.Any<User>()).Returns(true);
            _userRepository.SaveChangesAsync(Arg.Any<CancellationToken>()).Returns(1);
            _publishEndpoint.Publish(Arg.Any<UserCreatedEvent>(), Arg.Any<CancellationToken>())
                .Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotEqual(Guid.Empty, result);
        }


        [Fact]
        public async Task Handle_SavesProduct_WhenUserCreatedSuccessfully()
        {
            // Arrange
            var command = CreateValidCommand();
            _addressChecker.IsAddressValidAsync(Arg.Any<ShipmentAddress>(), Arg.Any<CancellationToken>())
                .Returns(true);
            _userRepository.Create(Arg.Any<User>()).Returns(true);
            _userRepository.SaveChangesAsync(Arg.Any<CancellationToken>()).Returns(1);
            _publishEndpoint.Publish(Arg.Any<UserCreatedEvent>(), Arg.Any<CancellationToken>())
                .Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            await _userRepository.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Handle_PublishesEvent_WhenUserCreatedSuccessfully()
        {
            // Arrange
            var command = CreateValidCommand();
            _addressChecker.IsAddressValidAsync(Arg.Any<ShipmentAddress>(), Arg.Any<CancellationToken>())
                .Returns(true);
            _userRepository.Create(Arg.Any<User>()).Returns(true);
            _userRepository.SaveChangesAsync(Arg.Any<CancellationToken>()).Returns(1);
            _publishEndpoint.Publish(Arg.Any<UserCreatedEvent>(), Arg.Any<CancellationToken>())
                .Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            await _publishEndpoint.Received(1).Publish(Arg.Any<UserCreatedEvent>(), Arg.Any<CancellationToken>());
        }
    }
}
