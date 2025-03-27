using Validata.Domain.Entities;
using Validata.Infrastructure.Infrastructure;

namespace Validata.Infrastructure.Repositories
{
    public interface IProductRepository : IGenericRepository<Product>
    {

    }

    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        public ProductRepository(IAppDbContext dbContext) : base(dbContext)
        {
        }
    }
}
