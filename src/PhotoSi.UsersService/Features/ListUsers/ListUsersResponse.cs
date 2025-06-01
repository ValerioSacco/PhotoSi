namespace PhotoSi.UsersService.Features.ListUsers
{
    public record GetShipmentAddressResponse(
        string country,
        string city,
        string street,
        string postalCode
    );

    public record ListUserResponse(
        Guid id,
        string firstname,
        string lastname,
        string username,
        string email,
        string? phoneNumber,
        string? profilePictureUrl,
        GetShipmentAddressResponse shipmentAddress
    );

    public record ListUsersResponse(
        int count, 
        int pageNumber, 
        int pageSize, 
        IList<ListUserResponse> users
    );
}
