using MediatR;
using PhotoSi.UsersService.Models;
using PhotoSi.UsersService.Repositories;
using PhotoSi.UsersService.Services;

namespace PhotoSi.UsersService.Features.CreateUser
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Guid>
    {
        private readonly IUserRepository _userRepository;
        private readonly IAddressChecker _addressChecker;

        public CreateUserCommandHandler(
            IUserRepository userRepository, 
            IAddressChecker addressChecker
        )
        {
            _userRepository = userRepository;
            _addressChecker = addressChecker;
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
                throw new Exception("Invalid shipment address provided. Check address book and pick an existing address.");
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
                await _userRepository.SaveChangesAsync(cancellationToken);
                return user.Id;
            }
            else
            {
                throw new Exception("Failed to create user");
            }

        }
    }

}
