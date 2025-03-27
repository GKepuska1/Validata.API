namespace Validata.Domain.Entities
{
    public class Customer : BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }

        public Customer(string firstName, string lastName, string address)
        {
            FirstName = firstName;
            LastName = lastName;
            Address = address;
        }

        public List<Order> Orders { get; set; }
    }
}
