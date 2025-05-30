using PhotoSi.Shared.Models;

namespace PhotoSi.UsersService.Models
{
    public class ShipmentAddress : BaseModel
    {
        public Guid Id { get; set; }
        public string Street { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string PostalCode { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public User User { get; set; } = null!;
    }

}
