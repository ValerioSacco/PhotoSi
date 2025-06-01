using MassTransit;
using NSubstitute;
using PhotoSi.Shared.Events;
using PhotoSi.Shared.Exceptions;
using PhotoSi.UsersService.Features.UpdateUser;
using PhotoSi.UsersService.Models;
using PhotoSi.UsersService.Repositories;
using PhotoSi.UsersService.Services;

namespace PhotoSi.UsersService.UnitTests.Features
{
    public class UpdateUserCommandHandlerTests
    {
        private readonly UpdateUserCommandHandler _handler;
        private readonly IUserRepository _userRepository = Substitute.For<IUserRepository>();
        private readonly IAddressChecker _addressChecker = Substitute.For<IAddressChecker>();
        private readonly IPublishEndpoint _publishEndpoint = Substitute.For<IPublishEndpoint>();

        public UpdateUserCommandHandlerTests()
        {
            _handler = new UpdateUserCommandHandler(_userRepository, _addressChecker, _publishEndpoint);
        }

        private static UpdateUserCommand CreateValidCommand(Guid userId)
        {
            return new UpdateUserCommand
            (
                userId,
                "newuser",
                "John",
                "Doe",
                "john.doe@example.com",
                "1234567890",
                "http://example.com/pic.jpg",
                new ShipmentAddressRequest
                (
                    "Country",
                    "City",
                    "12345",
                    "Street"
                )
            );
        }

        [Fact]
        public async Task Handle_ThrowsNotFoundException_WhenUserDoesNotExists()
        {
            // Arrange
            var command = CreateValidCommand(Guid.NewGuid());
            _userRepository.GetByIdAsync(command.id, Arg.Any<CancellationToken>())
                .Returns((User?)null);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() =>
                _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_ThrowsBusinessRuleException_WhenAddressIsInvalid()
        {
            // Arrange
            var command = CreateValidCommand(Guid.NewGuid());
            var user = new User { Id = command.id };
            _userRepository.GetByIdAsync(command.id, Arg.Any<CancellationToken>())
                .Returns(user);
            _addressChecker.IsAddressValidAsync(Arg.Any<ShipmentAddress>(), Arg.Any<CancellationToken>())
                .Returns(false);

            // Act & Assert
            await Assert.ThrowsAsync<BusinessRuleException>(() =>
                _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_ThrowsException_WhenUserUpdateFails()
        {
            // Arrange
            var command = CreateValidCommand(Guid.NewGuid());
            var user = new User { Id = command.id };
            _userRepository.GetByIdAsync(command.id, Arg.Any<CancellationToken>()).Returns(user);
            _addressChecker.IsAddressValidAsync(Arg.Any<ShipmentAddress>(), Arg.Any<CancellationToken>()).Returns(true);
            _userRepository.Update(user).Returns(false);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() =>
                _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_ReturnsProductId_WhenUserUpdatedSuccessfully()
        {
            // Arrange
            var command = CreateValidCommand(Guid.NewGuid());
            var user = new User { Id = command.id };
            _userRepository.GetByIdAsync(command.id, Arg.Any<CancellationToken>()).Returns(user);
            _addressChecker.IsAddressValidAsync(Arg.Any<ShipmentAddress>(), Arg.Any<CancellationToken>()).Returns(true);
            _userRepository.Update(user).Returns(true);
            _userRepository.SaveChangesAsync(Arg.Any<CancellationToken>()).Returns(1);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);
            // Assert
            Assert.NotEqual(Guid.Empty, result);
        }


        [Fact]
        public async Task Handle_SavesProduct_WhenUserUpdatedSuccessfully()
        {
            // Arrange
            var command = CreateValidCommand(Guid.NewGuid());
            var user = new User { Id = command.id };
            _userRepository.GetByIdAsync(command.id, Arg.Any<CancellationToken>()).Returns(user);
            _addressChecker.IsAddressValidAsync(Arg.Any<ShipmentAddress>(), Arg.Any<CancellationToken>()).Returns(true);
            _userRepository.Update(user).Returns(true);
            _userRepository.SaveChangesAsync(Arg.Any<CancellationToken>()).Returns(1);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            await _userRepository.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
        }


        [Fact]
        public async Task Handle_PublishesEvent_WhenUserUpdatedSuccessfully()
        {
            // Arrange
            var command = CreateValidCommand(Guid.NewGuid());
            var user = new User { Id = command.id };
            _userRepository.GetByIdAsync(command.id, Arg.Any<CancellationToken>()).Returns(user);
            _addressChecker.IsAddressValidAsync(Arg.Any<ShipmentAddress>(), Arg.Any<CancellationToken>()).Returns(true);
            _userRepository.Update(user).Returns(true);
            _userRepository.SaveChangesAsync(Arg.Any<CancellationToken>()).Returns(1);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            await _publishEndpoint.Received(1).Publish(
                Arg.Is<UserUpdatedEvent>(e =>
                    e.id == command.id &&
                    e.firstname == command.firstname &&
                    e.lastname == command.lastname &&
                    e.country == command.shipmentAddress.country &&
                    e.city == command.shipmentAddress.city &&
                    e.street == command.shipmentAddress.street &&
                    e.postalCode == command.shipmentAddress.postalCode
                ),
                Arg.Any<CancellationToken>());
        }
    }
}
