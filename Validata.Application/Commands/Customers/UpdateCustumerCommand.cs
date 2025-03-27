using MediatR;
using Validata.Infrastructure.Repositories;

namespace Validata.Application.Commands.Customers
{
    public class UpdateCustomerCommand : IRequest<bool>
    {
        public int Id { get; }
        public string FirstName { get; }
        public string LastName { get; }
        public string Address { get; }

        public UpdateCustomerCommand(int id, string firstName, string lastName, string address)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Address = address;
        }

        public class UpdateCustomerCommandHandler : IRequestHandler<UpdateCustomerCommand, bool>
        {
            private readonly IUnitOfWork _unitOfWork;
            public UpdateCustomerCommandHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<bool> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
            {
                var customer = await _unitOfWork.Customers.GetByIdAsync(request.Id);
                if (customer == null)
                {
                    return false;
                }

                customer.FirstName = request.FirstName;
                customer.LastName = request.LastName;
                customer.Address = request.Address;

                await _unitOfWork.Customers.UpdateAsync(customer);
                await _unitOfWork.CompleteAsync();

                return true;
            }
        }

    }

}
