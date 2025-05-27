namespace PhotoSi.ProductsService.Models
{
    public sealed class Category
    {
        public required int Code { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
