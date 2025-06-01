using MassTransit;
using PhotoSi.OrdersService.Repositories;
using PhotoSi.Shared.Events;

namespace PhotoSi.OrdersService.Events
{
    public class UserDeletedEventConsumer : IConsumer<UserDeletedEvent>
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<UserDeletedEventConsumer> _logger;

        public UserDeletedEventConsumer(
            IUserRepository userRepository, 
            ILogger<UserDeletedEventConsumer> logger
        )
        {
            _userRepository = userRepository;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<UserDeletedEvent> context)
        {
            var user = await _userRepository
                .GetByIdAsync(context.Message.id, context.CancellationToken);

            if (user is null)
            {
                _logger.LogWarning($"User not found: {context.Message.id}");
                return;
            }

            user.IsAvailable = false;
            var updated = _userRepository.Update(user);

            if (updated)
            {
                await _userRepository
                    .SaveChangesAsync(context.CancellationToken);
                _logger.LogInformation($"User set as unavailable successfully: {user.Id}");
            }
            else
            {
                _logger.LogError($"Failed to set user as unavailable: {user.Id}");
                throw new Exception("Failed to set user unavailable in the database.");
            }
        }
    }
}
