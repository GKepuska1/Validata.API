namespace Validata.Domain.Entities
{
    public class Product : BaseEntity
    {
        public string Name { get; set; }
        public decimal Price { get; set; }

        public Product(string name, decimal price)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Product name is required.");
            if (price <= 0) throw new ArgumentException("Price must be greater than zero.");

            Name = name;
            Price = price;
        }
    }
}
