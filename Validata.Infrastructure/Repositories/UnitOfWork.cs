using Validata.Infrastructure.Infrastructure;

namespace Validata.Infrastructure.Repositories
{
    public interface IUnitOfWork
    {
        IOrderRepository Orders { get; }
        IOrderItemRepository OrderItems { get; }
        ICustomerRepository Customers { get; }
        IProductRepository Products { get; }

        Task<int> CompleteAsync();
    }

    public class UnitOfWork : IUnitOfWork
    {
        private readonly IAppDbContext _dbContext;

        public IOrderRepository Orders { get; }
        public IOrderItemRepository OrderItems { get; }
        public ICustomerRepository Customers { get; }
        public IProductRepository Products { get; }

        public UnitOfWork(IAppDbContext dbContext)
        {
            _dbContext = dbContext;
            Orders = new OrderRepository(_dbContext);
            OrderItems = new OrderItemRepository(_dbContext);
            Customers = new CustomerRepository(_dbContext);
            Products = new ProductRepository(_dbContext);
        }

        public async Task<int> CompleteAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }
    }
}
