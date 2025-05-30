using MediatR;
using PhotoSi.Shared.Exceptions;
using PhotoSi.UsersService.Repositories;

namespace PhotoSi.UsersService.Features.GetUser
{
    public class GetUserQueryHandler : IRequestHandler<GetUserQuery, GetUserResponse>
    {
        private readonly IUserRepository _userRepository;

        public GetUserQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<GetUserResponse> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepository
                .GetByIdAsync(request.id, cancellationToken);

            if (user is null)
            {
                throw new NotFoundException("The user requested does not exist.");
            }

            return new GetUserResponse(
                user.Id,
                user.FirstName,
                user.LastName,
                user.UserName,
                user.Email,
                user.PhoneNumber,
                user.ProfilePictureUrl,
                new GetShipmentAddressResponse(
                    user.ShipmentAddress.Country,
                    user.ShipmentAddress.City,
                    user.ShipmentAddress.Street,
                    user.ShipmentAddress.PostalCode
                )
            );
        }
    }
}
