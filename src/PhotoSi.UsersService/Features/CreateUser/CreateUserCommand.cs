using MediatR;

namespace PhotoSi.UsersService.Features.CreateUser
{
    public record ShipmentAddressRequest(
        string country, 
        string city, 
        string postalCode, 
        string street
    );

    public record CreateUserCommand(
        string username, 
        string firstname, 
        string lastname, 
        string email,
        ShipmentAddressRequest shipmentAddress
    ) : IRequest<Guid>;
}
