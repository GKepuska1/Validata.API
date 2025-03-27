using MediatR;
using Validata.Domain.Entities;
using Validata.Infrastructure.Repositories;

namespace Validata.Application.Commands.Customers
{
    public class CreateCustomerCommand : IRequest<Customer>
    {
        public string FirstName { get; }
        public string LastName { get; }
        public string Address { get; }

        public CreateCustomerCommand(string firstName, string lastName, string address)
        {
            FirstName = firstName;
            LastName = lastName;
            Address = address;
        }

        public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, Customer>
        {
            private readonly IUnitOfWork _unitOfWork;

            public CreateCustomerCommandHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<Customer> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
            {
                var customer = new Customer(
                    firstName: request.FirstName,
                    lastName: request.LastName,
                    address: request.Address
                );

                await _unitOfWork.Customers.AddAsync(customer);
                await _unitOfWork.CompleteAsync();

                return customer;
            }
        }
    }
}
