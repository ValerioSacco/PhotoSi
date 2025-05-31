using PhotoSi.Shared.Models;

namespace PhotoSi.OrdersService.Models
{
    public class OrderLine : BaseModel
    {
        public Guid Id { get; set; }
        public string? Notes { get; set; }
        public int Quantity { get; set; }
        public Guid OrderId { get; set; }
        public Order Order { get; set; } = null!;
        public Guid ProductId { get; set; }
        public Product Product { get; set; } = null!;

    }
}
