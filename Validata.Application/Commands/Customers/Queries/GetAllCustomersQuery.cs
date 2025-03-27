using MediatR;
using Validata.Domain.Entities;
using Validata.Infrastructure.Repositories;

namespace Validata.Application.Commands.Customers.Queries
{
    public class GetAllCustomersQuery : IRequest<IEnumerable<Customer>>
    {
        public GetAllCustomersQuery() { }

        public class GetAllCustomersQueryHandler : IRequestHandler<GetAllCustomersQuery, IEnumerable<Customer>>
        {
            private readonly IUnitOfWork _unitOfWork;

            public GetAllCustomersQueryHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<IEnumerable<Customer>> Handle(GetAllCustomersQuery request, CancellationToken cancellationToken)
            {
                return await _unitOfWork.Customers.GetAllAsync();
            }
        }

    }
}
