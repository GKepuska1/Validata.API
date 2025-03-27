namespace Validata.Domain.Entities
{
    public class OrderItem
    {
        public OrderItem(decimal price, int quantity, int productId)
        {
            if (price <= 0) throw new ArgumentException("Price must be greater than zero.");
            if (quantity <= 0) throw new ArgumentException("Quantity must be greater than zero.");
            if (productId <= 0) throw new ArgumentException("productId must be greater than zero.");

            Quantity = quantity;
            Price = Price;
            ProductId = productId;
        }

        public int ProductId { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public int OrderId { get; set; }
        public decimal TotalPrice => Product.Price * Quantity;


        public Order Order { get; set; }
        public Product Product { get; set; }
    }
}
