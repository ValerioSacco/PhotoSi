using MediatR;
using PhotoSi.UsersService.Repositories;

namespace PhotoSi.UsersService.Features.ListUsers
{
    public class ListUsersQueryHandler : IRequestHandler<ListUsersQuery, ListUsersResponse>
    {
        private readonly IUserRepository _userRespository;

        public ListUsersQueryHandler(IUserRepository userRespository)
        {
            _userRespository = userRespository;
        }

        public async Task<ListUsersResponse> Handle(
            ListUsersQuery request, 
            CancellationToken cancellationToken
        )
        {
            var totalCount = await _userRespository
                .CountAsync(cancellationToken);

            var users = await _userRespository
                .ListAllAsync(request.pageNumber, request.pageSize, cancellationToken);

            return new ListUsersResponse
            (
                totalCount,
                request.pageNumber,
                request.pageSize,
                users.Select(u => new ListUserResponse
                (
                    u.Id,
                    u.FirstName,
                    u.LastName,
                    u.UserName,
                    u.Email,
                    u.PhoneNumber,
                    u.ProfilePictureUrl,
                    new GetShipmentAddressResponse
                    (
                        u.ShipmentAddress?.Country ?? string.Empty,
                        u.ShipmentAddress?.City ?? string.Empty,
                        u.ShipmentAddress?.Street ?? string.Empty,
                        u.ShipmentAddress?.PostalCode ?? string.Empty
                    )
                )).ToList()
            );

        }
    }
}
