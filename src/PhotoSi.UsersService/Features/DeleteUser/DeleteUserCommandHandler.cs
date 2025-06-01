using MassTransit;
using MediatR;
using PhotoSi.Shared.Events;
using PhotoSi.Shared.Exceptions;
using PhotoSi.UsersService.Repositories;

namespace PhotoSi.UsersService.Features.DeleteUser
{
    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPublishEndpoint _publishEndpoint;

        public DeleteUserCommandHandler(
            IUserRepository userRepository, 
            IPublishEndpoint publishEndpoint
        )
        {
            _userRepository = userRepository;
            _publishEndpoint = publishEndpoint;
        }

        public async Task Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.id, cancellationToken);

            if (user is null)
            {
                throw new NotFoundException("The user requested does not exist.");
            }

            if (_userRepository.Delete(user))
            {
                await _userRepository
                    .SaveChangesAsync(cancellationToken);
                //I should send the event to an outbox before saving the changes
                await _publishEndpoint.Publish(
                    new UserDeletedEvent(user.Id),
                    cancellationToken
                );
            }
            else
            {
                throw new Exception("Failed to delete user");
            }
        }
    }
}
