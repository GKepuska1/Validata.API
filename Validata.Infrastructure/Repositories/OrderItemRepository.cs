using Microsoft.EntityFrameworkCore;
using Validata.Domain.Entities;
using Validata.Infrastructure.Infrastructure;

namespace Validata.Infrastructure.Repositories
{

    public interface IOrderItemRepository : IGenericRepository<OrderItem>
    {
        Task<IEnumerable<OrderItem>> GetByOrderId(int orderId);
    }

    public class OrderItemRepository : GenericRepository<OrderItem>, IOrderItemRepository
    {
        public OrderItemRepository(IAppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<OrderItem>> GetByOrderId(int orderId)
        {
            var orders = await _context.OrderItems.Where(x => x.OrderId == orderId)
                                              .ToListAsync();
            return orders;
        }
    }
}
