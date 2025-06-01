using MediatR;

namespace PhotoSi.UsersService.Features.CreateUser
{
    public record CreateShipmentAddressRequest(
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
        string profilePictureUrl,
        string phoneNumber,
        CreateShipmentAddressRequest shipmentAddress
    ) : IRequest<Guid>;
}
