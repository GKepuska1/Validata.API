using MediatR;
using Validata.Infrastructure.Repositories;

namespace Validata.Application.Commands.Orders
{
    public class DeleteOrderCommand : IRequest<bool>
    {
        public int Id { get; }

        public DeleteOrderCommand(int id)
        {
            Id = id;
        }

        public class DeleteOrderCommandHandler : IRequestHandler<DeleteOrderCommand, bool>
        {
            private readonly IUnitOfWork _unitOfWork;

            public DeleteOrderCommandHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<bool> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
            {
                var order = await _unitOfWork.Orders.GetByIdAsync(request.Id);
                if (order == null)
                {
                    return false;
                }

                await _unitOfWork.Orders.DeleteAsync(order);
                await _unitOfWork.CompleteAsync();

                return true;
            }
        }
    }
}
