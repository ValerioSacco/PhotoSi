using PhotoSi.Shared.Models;

namespace PhotoSi.OrdersService.Models
{
    public class Product : BaseModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public bool IsAvailable { get; set; } = true;

    }
}
