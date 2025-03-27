using Validata.Domain.Entities;
using Validata.Infrastructure.Infrastructure;

namespace Validata.Infrastructure.Repositories
{
    public interface ICustomerRepository : IGenericRepository<Customer>
    {

    }

    public class CustomerRepository : GenericRepository<Customer>, ICustomerRepository
    {
        public CustomerRepository(IAppDbContext dbContext) : base(dbContext)
        {
        }
    }
}
