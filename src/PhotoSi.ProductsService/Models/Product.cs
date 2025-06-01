using PhotoSi.Shared.Models;

namespace PhotoSi.ProductsService.Models
{
    public class Product : BaseModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public Guid CategoryId { get; set; }
        public Category Category { get; set; } = null!;
    }
}
