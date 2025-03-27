using MediatR;
using Validata.Domain.Entities;
using Validata.Infrastructure.Repositories;

namespace Validata.Application.Commands.Orders.Queries
{
    public class GetAllOrdersQuery : IRequest<IEnumerable<Order>>
    {
        public GetAllOrdersQuery() { }

        public class GetAllOrdersQueryHandler : IRequestHandler<GetAllOrdersQuery, IEnumerable<Order>>
        {
            private readonly IUnitOfWork _unitOfWork;

            public GetAllOrdersQueryHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<IEnumerable<Order>> Handle(GetAllOrdersQuery request, CancellationToken cancellationToken)
            {
                var orders = await _unitOfWork.Orders.GetAllAsync();
                return orders;
            }
        }
    }
}
