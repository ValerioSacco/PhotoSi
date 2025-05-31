using PhotoSi.Shared.Models;

namespace PhotoSi.OrdersService.Models
{
    public class Order : BaseModel
    {
        public Guid Id { get; set; }
        public string Currency { get; set; } = "EUR";
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;
        public ICollection<OrderLine> OrderLines { get; set; } = new List<OrderLine>();

    }
}
