using MediatR;
using Validata.Domain.Entities;
using Validata.Infrastructure.Repositories;

namespace Validata.Application.Commands.Products.Queries
{
    public class GetProductByIdQuery : IRequest<Product>
    {
        public int Id { get; }

        public GetProductByIdQuery(int id)
        {
            Id = id;
        }

        public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, Product>
        {
            private readonly IUnitOfWork _unitOfWork;

            public GetProductByIdQueryHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<Product> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
            {
                return await _unitOfWork.Products.GetByIdAsync(request.Id);
            }
        }
    }
}
