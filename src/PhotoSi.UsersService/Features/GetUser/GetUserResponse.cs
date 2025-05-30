namespace PhotoSi.UsersService.Features.GetUser
{
    public record GetShipmentAddressResponse(
        string country,
        string city,
        string street,
        string postalCode
    );

    public record GetUserResponse(
        Guid id, 
        string firstname, 
        string lastname, 
        string username, 
        string email,
        string? phoneNumber,
        string? profilePictureUrl,
        GetShipmentAddressResponse shipmentAddress
    );

}
