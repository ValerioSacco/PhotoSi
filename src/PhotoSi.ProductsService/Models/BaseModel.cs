namespace PhotoSi.ProductsService.Models
{
    public abstract class BaseModel
    {
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public int Version { get; set; } = 1;

    }
}
