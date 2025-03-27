using MediatR;
using Validata.Infrastructure.Repositories;

namespace Validata.API.Controllers
{
    public class DeleteCustomerCommand : IRequest<bool>
    {
        public int Id { get; }

        public DeleteCustomerCommand(int id)
        {
            Id = id;
        }

        public class DeleteCustomerCommandHandler : IRequestHandler<DeleteCustomerCommand, bool>
        {
            private readonly IUnitOfWork _unitOfWork;

            public DeleteCustomerCommandHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<bool> Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
            {
                var customer = await _unitOfWork.Customers.GetByIdAsync(request.Id);
                if (customer == null)
                {
                    return false;
                }

                await _unitOfWork.Customers.DeleteAsync(customer);
                await _unitOfWork.CompleteAsync();

                return true;
            }
        }

    }
}
