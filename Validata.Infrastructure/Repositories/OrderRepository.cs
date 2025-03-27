using Microsoft.EntityFrameworkCore;
using Validata.Domain.Entities;
using Validata.Infrastructure.Infrastructure;

namespace Validata.Infrastructure.Repositories
{
    public interface IOrderRepository : IGenericRepository<Order>
    {
        Task<IEnumerable<Order>> GetCustomerOrdersByDateAsync(int customerId);
    }

    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        public OrderRepository(IAppDbContext context) : base(context)
        {

        }

        public async Task<IEnumerable<Order>> GetCustomerOrdersByDateAsync(int customerId)
        {
            var orders = await _context.Orders.Where(x => x.CustomerId == customerId)
                                              .OrderByDescending(x => x.OrderDate)
                                              .ToListAsync();
            return orders;            
        }
    }
}
