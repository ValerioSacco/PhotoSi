using PhotoSi.Shared.Models;

namespace PhotoSi.OrdersService.Models
{
    public class User : BaseModel
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public ShipmentAddress Address { get; set; } = null!;
        public bool IsAvailable { get; set; } = true;

    }
}
