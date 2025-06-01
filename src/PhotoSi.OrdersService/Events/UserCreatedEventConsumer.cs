using MassTransit;
using PhotoSi.OrdersService.Models;
using PhotoSi.OrdersService.Repositories;
using PhotoSi.Shared.Events;

namespace PhotoSi.OrdersService.Events
{
    public class UserCreatedEventConsumer : IConsumer<UserCreatedEvent>
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<UserCreatedEventConsumer> _logger;

        public UserCreatedEventConsumer(
            IUserRepository userRepository, 
            ILogger<UserCreatedEventConsumer> logger
        )
        {
            _userRepository = userRepository;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<UserCreatedEvent> context)
        {
            var user = new User
            {
                Id = context.Message.id,
                FirstName = context.Message.firstname,
                LastName = context.Message.lastname,
                Address = new ShipmentAddress
                {
                    Country = context.Message.country,
                    City = context.Message.city,
                    PostalCode = context.Message.postalCode,
                    Street = context.Message.street
                }
            };

            var created = _userRepository.Create(user);

            if (created)
            {
                await _userRepository
                    .SaveChangesAsync(context.CancellationToken);
                _logger.LogInformation($"User created successfully: {user.Id}");
            }
            else
            {
                _logger.LogError($"Failed to create user: {user.Id}");
                throw new Exception("Failed to create user in the database.");
            }
        }
    }
}
