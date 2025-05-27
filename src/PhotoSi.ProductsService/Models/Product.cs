namespace PhotoSi.ProductsService.Models
{
    public sealed class Product
    {
        public required int Code { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required int CategoryCode { get; set; }
        public Category Category { get; set; }
    }
}
