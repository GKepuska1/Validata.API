namespace Validata.Domain.Entities
{
    public class Order : BaseEntity
    {
        public int CustomerId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalPrice { get; set; }

        public Customer Customer { get; set; }
        public List<OrderItem> OrderItems { get; set; }
    }
}
