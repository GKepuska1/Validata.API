using MediatR;
using Validata.Domain.Entities;
using Validata.Infrastructure.Repositories;

namespace Validata.Application.Commands.Customers.Queries
{
    public class GetCustomerByIdQuery : IRequest<Customer>
    {
        public int Id { get; }

        public GetCustomerByIdQuery(int id)
        {
            Id = id;
        }

        public class GetCustomerByIdQueryHandler : IRequestHandler<GetCustomerByIdQuery, Customer>
        {
            private readonly IUnitOfWork _unitOfWork;

            public GetCustomerByIdQueryHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<Customer> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
            {
                return await _unitOfWork.Customers.GetByIdAsync(request.Id);
            }
        }
    }
}
