namespace PhotoSi.OrdersService.Models
{
    public class UserInfo
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public ShipmentAddress Address { get; set; } = null!;

    }
}
