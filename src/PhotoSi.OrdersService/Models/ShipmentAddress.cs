namespace PhotoSi.OrdersService.Models
{
    public class ShipmentAddress
    {
        public Guid UserId { get; set; }
        public string Country { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Street { get; set; } = string.Empty;
        public string PostalCode { get; set; } = string.Empty;

    }
}
