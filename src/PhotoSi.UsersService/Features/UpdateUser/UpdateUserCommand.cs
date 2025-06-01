using MediatR;

namespace PhotoSi.UsersService.Features.UpdateUser
{
    public record UpdateShipmentAddressRequest(
        string country, 
        string city, 
        string postalCode, 
        string street
    );

    public record UpdateUserCommand(
        Guid id,
        string username, 
        string firstname, 
        string lastname, 
        string email,
        string profilePictureUrl,
        string phoneNumber,
        UpdateShipmentAddressRequest shipmentAddress
    ) : IRequest<Guid>;
}
