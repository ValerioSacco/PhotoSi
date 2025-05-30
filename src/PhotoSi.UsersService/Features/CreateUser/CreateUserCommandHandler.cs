using MediatR;
using PhotoSi.UsersService.Models;
using PhotoSi.UsersService.Repositories;

namespace PhotoSi.UsersService.Features.CreateUser
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Guid>
    {
        private readonly IUserRepository _userRepository;

        public CreateUserCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<Guid> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var user = new User()
            {
                Id = Guid.NewGuid(),
                UserName = request.username,
                FirstName = request.firstname,
                LastName = request.lastname,
                Email = request.email,
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
