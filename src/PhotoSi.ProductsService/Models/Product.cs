namespace PhotoSi.ProductsService.Models
{
    public class Product : BaseModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
