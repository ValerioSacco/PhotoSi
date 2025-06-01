using MassTransit;
using PhotoSi.OrdersService.Repositories;
using PhotoSi.Shared.Events;

namespace PhotoSi.OrdersService.Events
{
    public class UserUpdatedEventConsumer : IConsumer<UserUpdatedEvent>
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<UserUpdatedEventConsumer> _logger;

        public UserUpdatedEventConsumer(
            IUserRepository userRepository,
            ILogger<UserUpdatedEventConsumer> logger
        )
        {
            _userRepository = userRepository;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<UserUpdatedEvent> context)
        {
            var user = await _userRepository
                .GetByIdAsync(context.Message.id, context.CancellationToken);

            if (user is null)
            {
                _logger.LogWarning($"User not found: {context.Message.id}");
                return;
            }

            user.FirstName = context.Message.firstname;
            user.LastName = context.Message.lastname;
            user.Address.Country = context.Message.country;
            user.Address.City = context.Message.city;
            user.Address.Street = context.Message.street;
            user.Address.PostalCode = context.Message.postalCode;

            var updated = _userRepository.Update(user);

            if (updated)
            {
                await _userRepository
                    .SaveChangesAsync(context.CancellationToken);
                _logger.LogInformation($"User updated successfully: {user.Id}");
            }
            else
            {
                _logger.LogError($"Failed to update user: {user.Id}");
                throw new Exception("Failed to update user in the database.");
            }
        }
    }
}
