using MassTransit;
using MediatR;
using PhotoSi.Shared.Events;
using PhotoSi.Shared.Exceptions;
using PhotoSi.UsersService.Models;
using PhotoSi.UsersService.Repositories;
using PhotoSi.UsersService.Services;

namespace PhotoSi.UsersService.Features.CreateUser
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Guid>
    {
        private readonly IUserRepository _userRepository;
        private readonly IAddressChecker _addressChecker;
        private readonly IPublishEndpoint _publishEndpoint;

        public CreateUserCommandHandler(
            IUserRepository userRepository,
            IAddressChecker addressChecker,
            IPublishEndpoint publishEndpoint)
        {
            _userRepository = userRepository;
            _addressChecker = addressChecker;
            _publishEndpoint = publishEndpoint;
        }

        public async Task<Guid> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var addressValid = await _addressChecker.IsAddressValidAsync(
                new ShipmentAddress()
                {
                    Country = request.shipmentAddress.country,
                    City = request.shipmentAddress.city,
                    PostalCode = request.shipmentAddress.postalCode,
                    Street = request.shipmentAddress.street
                },
                cancellationToken
            );

            if (!addressValid)
            {
                throw new BusinessRuleException("Invalid shipment address provided. Check address book and pick an existing address.");
            }

            var user = new User()
            {
                Id = Guid.NewGuid(),
                UserName = request.username,
                FirstName = request.firstname,
                LastName = request.lastname,
                Email = request.email,
                PhoneNumber = request.phoneNumber,
                ProfilePictureUrl = request.profilePictureUrl,
                ShipmentAddress = new ShipmentAddress()
                {
                    Country = request.shipmentAddress.country,
                    City = request.shipmentAddress.city,
                    PostalCode = request.shipmentAddress.postalCode,
                    Street = request.shipmentAddress.street
                }
            };

            if (_userRepository.Create(user))
            {
                //I should send the event to an outbox
                await _publishEndpoint.Publish(
                    new UserCreatedEvent(
                        user.Id,
                        user.FirstName,
                        user.LastName,
                        user.ShipmentAddress.Country,
                        user.ShipmentAddress.City,
                        user.ShipmentAddress.Street,
                        user.ShipmentAddress.PostalCode
                    ), cancellationToken);
                await _userRepository
                    .SaveChangesAsync(cancellationToken);
                return user.Id;
            }
            else
            {
                throw new Exception("Failed to create user");
            }

        }
    }

}
