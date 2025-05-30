using PhotoSi.Shared.Models;

namespace PhotoSi.ProductsService.Models
{
    public class Category : BaseModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
