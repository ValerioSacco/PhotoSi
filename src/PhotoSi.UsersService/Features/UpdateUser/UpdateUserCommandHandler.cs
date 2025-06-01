using MassTransit;
using MediatR;
using PhotoSi.Shared.Events;
using PhotoSi.Shared.Exceptions;
using PhotoSi.UsersService.Models;
using PhotoSi.UsersService.Repositories;
using PhotoSi.UsersService.Services;

namespace PhotoSi.UsersService.Features.UpdateUser
{
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, Guid>
    {
        private readonly IUserRepository _userRepository;
        private readonly IAddressChecker _addressChecker;
        private readonly IPublishEndpoint _publishEndpoint;

        public UpdateUserCommandHandler(
            IUserRepository userRepository,
            IAddressChecker addressChecker,
            IPublishEndpoint publishEndpoint)
        {
            _userRepository = userRepository;
            _addressChecker = addressChecker;
            _publishEndpoint = publishEndpoint;
        }

        public async Task<Guid> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.id, cancellationToken);

            if (user is null)
            {
                throw new NotFoundException($"User with id {request.id} does not exist.");
            }

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

            user.UserName = request.username;
            user.FirstName = request.firstname;
            user.LastName = request.lastname;
            user.Email = request.email;
            user.PhoneNumber = request.phoneNumber;
            user.ProfilePictureUrl = request.profilePictureUrl;
            user.ShipmentAddress = new ShipmentAddress()
            {
                Country = request.shipmentAddress.country,
                City = request.shipmentAddress.city,
                PostalCode = request.shipmentAddress.postalCode,
                Street = request.shipmentAddress.street
            };

            if (_userRepository.Update(user))
            {
                await _userRepository
                    .SaveChangesAsync(cancellationToken);
                //I should send the event to an outbox before saving the changes
                await _publishEndpoint.Publish(
                    new UserUpdatedEvent(
                        user.Id,
                        user.FirstName,
                        user.LastName,
                        user.ShipmentAddress.Country,
                        user.ShipmentAddress.City,
                        user.ShipmentAddress.Street,
                        user.ShipmentAddress.PostalCode
                    ), cancellationToken);

                return user.Id;
            }
            else
            {
                throw new Exception("Failed to update user");
            }

        }
    }

}
