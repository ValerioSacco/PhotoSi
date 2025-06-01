using MassTransit;
using NSubstitute;
using PhotoSi.Shared.Events;
using PhotoSi.Shared.Exceptions;
using PhotoSi.UsersService.Features.DeleteUser;
using PhotoSi.UsersService.Models;
using PhotoSi.UsersService.Repositories;

namespace PhotoSi.UsersService.UnitTests.Features
{
    public class DeleteUserCommandHandlerTests
    {
        private DeleteUserCommandHandler _handler;
        private readonly IUserRepository _userRepository = Substitute.For<IUserRepository>();
        private readonly IPublishEndpoint _publishEndpoint = Substitute.For<IPublishEndpoint>();

        public DeleteUserCommandHandlerTests()
        {
            _handler = new DeleteUserCommandHandler(_userRepository, _publishEndpoint);
        }

        private static DeleteUserCommand CreateValidCommand(Guid id)
        {
            return new DeleteUserCommand(id);
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
        public async Task Handle_ThrowsException_WhenUserDeletionFails()
        {
            // Arrange
            var command = CreateValidCommand(Guid.NewGuid());
            var user = new User { Id = command.id, UserName = "TestUser" };
            _userRepository.GetByIdAsync(command.id, Arg.Any<CancellationToken>())
                .Returns(user);

            _userRepository.Delete(user).Returns(false);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() =>
                _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_RemovesUser_WhenUserDeletedSuccessfully()
        {
            // Arrange
            var command = CreateValidCommand(Guid.NewGuid());
            var user = new User { Id = command.id, UserName = "TestUser" };
            _userRepository.GetByIdAsync(command.id, Arg.Any<CancellationToken>())
                .Returns(user);

            _userRepository.Delete(user).Returns(true);
            _userRepository.SaveChangesAsync(Arg.Any<CancellationToken>()).Returns(1);
            _publishEndpoint.Publish(Arg.Any<UserDeletedEvent>(), Arg.Any<CancellationToken>())
                .Returns(Task.CompletedTask);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            await _userRepository.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
        }


        [Fact]
        public async Task Handle_PublishesEvent_WhenProductDeletedSuccessfully()
        {
            // Arrange
            var command = CreateValidCommand(Guid.NewGuid());
            var user = new User { Id = command.id, UserName = "TestUser" };
            _userRepository.GetByIdAsync(command.id, Arg.Any<CancellationToken>())
                .Returns(user);

            _userRepository.Delete(user).Returns(true);
            _userRepository.SaveChangesAsync(Arg.Any<CancellationToken>()).Returns(1);
            _publishEndpoint.Publish(Arg.Any<UserDeletedEvent>(), Arg.Any<CancellationToken>())
                .Returns(Task.CompletedTask);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            //await _userRepository.Received(1).GetByIdAsync(userId, Arg.Any<CancellationToken>());
            //_userRepository.Received(1).Delete(user);
            //await _userRepository.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
            await _publishEndpoint.Received(1).Publish(
                Arg.Is<UserDeletedEvent>(e => e.id == user.Id),
                Arg.Any<CancellationToken>());
        }

    }
}
