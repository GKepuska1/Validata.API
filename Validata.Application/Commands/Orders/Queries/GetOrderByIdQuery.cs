using MediatR;
using Validata.Domain.Entities;
using Validata.Infrastructure.Repositories;

namespace Validata.Application.Commands.Orders.Queries
{
    public class GetOrderByIdQuery : IRequest<Order>
    {
        public int Id { get; }

        public GetOrderByIdQuery(int id)
        {
            Id = id;
        }
        public class GetOrderByIdQueryHandler : IRequestHandler<GetOrderByIdQuery, Order>
        {
            private readonly IUnitOfWork _unitOfWork;

            public GetOrderByIdQueryHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<Order> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
            {
                var order = await _unitOfWork.Orders.GetByIdAsync(request.Id);
                return order;
            }
        }

    }
}
