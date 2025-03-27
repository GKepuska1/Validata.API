namespace Validata.Domain.Entities
{
    public class Order : BaseEntity
    {
        public int CustomerId { get; set; }
        public List<OrderItem> Items { get; set; } = new();
        public DateTime OrderDate { get; set; }
        public decimal TotalPrice => Items.Sum(i => i.TotalPrice);

        public Order(List<OrderItem> items)
        {
            if (items == null || items.Count == 0) throw new ArgumentException("Order must contain at least one item.");

            Items = items;
            OrderDate = DateTime.UtcNow;
        }

        public Customer Customer { get; set; }
    }
}
