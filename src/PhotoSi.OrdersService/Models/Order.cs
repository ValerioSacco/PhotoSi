using PhotoSi.Shared.Models;

namespace PhotoSi.OrdersService.Models
{
    public class Order : BaseModel
    {
        public Guid Id { get; set; }
        public string Currency { get; set; } = "EUR";
        public UserInfo User { get; set; } = null!;
        public ICollection<OrderLineProduct> Products { get; set; } = new List<OrderLineProduct>();

    }
}
