namespace PhotoSi.UsersService.Models
{
    public class User : BaseModel
    {
        public Guid Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? ProfilePictureUrl { get; set; } = null;
        public string? PhoneNumber { get; set; } = null;
        public Guid ShipmentAddressId { get; set; }
        public ShipmentAddress ShipmentAddress { get; set; } = null!;

    }
}
