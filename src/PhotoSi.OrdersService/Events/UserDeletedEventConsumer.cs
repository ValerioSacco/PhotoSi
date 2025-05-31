using MassTransit;
using PhotoSi.OrdersService.Repositories;
using PhotoSi.Shared.Events;

namespace PhotoSi.OrdersService.Events
{
    public class UserDeletedEventConsumer : IConsumer<UserDeletedEvent>
    {
        private readonly IUserRepository _userRepository;

        public UserDeletedEventConsumer(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task Consume(ConsumeContext<UserDeletedEvent> context)
        {
            var user = await _userRepository
                .GetByIdAsync(context.Message.id, context.CancellationToken);

            if (user == null)
            {
                return;
            }

            user.IsAvailable = false;
            var updated = _userRepository.Update(user);

            if (updated)
            {
                await _userRepository
                    .SaveChangesAsync(context.CancellationToken);
            }
            else
            {
                throw new Exception("Failed to set user unavailable in the database.");
            }
        }
    }
}
