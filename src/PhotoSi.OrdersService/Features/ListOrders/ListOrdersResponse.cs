namespace PhotoSi.OrdersService.Features.ListOrders
{
    public record class ListOrderLineProductResponse(
        string name,
        string description,
        decimal price,
        string categoryName
    );

    public record class ListOrderLineResponse(
        Guid orderLineId,
        int quantity,
        string notes,
        ListOrderLineProductResponse product
    );

    public record class ListOrderUserAddressResponse(
        string street,
        string city,
        string country,
        string postalCode
    );

    public record ListOrderUserResponse(
        string firstName,
        string lastName,
        ListOrderUserAddressResponse shipmentAddress
    );

    public record ListOrderResponse(
        Guid id,
        string currency,
        decimal totalAmount,
        ListOrderUserResponse user,
        IList<ListOrderLineResponse> orderLines
    );

    public record ListOrdersResponse(
        int count, 
        int pageNumber, 
        int pageSize, 
        IList<ListOrderResponse> orders
    );
}
