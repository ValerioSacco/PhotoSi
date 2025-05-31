namespace PhotoSi.OrdersService.Features.GetOrder
{
    public record class GetOrderLineProductResponse(
        string name,
        string description,
        decimal price,
        string categoryName
    );

    public record class GetOrderLineResponse(
        int quantity,
        GetOrderLineProductResponse product
    );

    public record class GetOrderUserAddressResponse(
        string street,
        string city,
        string country,
        string postalCode
    );

    public record GetOrderUserResponse(
        string firstName,
        string lastName,
        GetOrderUserAddressResponse shipmentAddress
    );

    public record GetOrderResponse(
        Guid id, 
        string currency,
        decimal totalAmount,
        GetOrderUserResponse user,
        IList<GetOrderLineResponse> orderLines
    );
}
