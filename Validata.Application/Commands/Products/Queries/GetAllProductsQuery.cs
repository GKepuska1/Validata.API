using MediatR;
using Validata.Domain.Entities;
using Validata.Infrastructure.Repositories;

namespace Validata.Application.Commands.Products.Queries
{
    public class GetAllProductsQuery : IRequest<IEnumerable<Product>>
    {
        public GetAllProductsQuery() { }

        public class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQuery, IEnumerable<Product>>
        {
            private readonly IUnitOfWork _unitOfWork;

            public GetAllProductsQueryHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<IEnumerable<Product>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
            {
                return await _unitOfWork.Products.GetAllAsync();
            }
        }

    }
}
