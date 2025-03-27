using MediatR;
using Validata.Domain.Entities;
using Validata.Infrastructure.Repositories;

namespace Validata.Application.Commands.Products
{
    public class CreateProductCommand : IRequest<Product>
    {
        public string Name { get; set; }
        public decimal Price { get; set; }

        public CreateProductCommand(string name, decimal price)
        {
            Name = name;
            Price = price;
        }

        public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Product>
        {
            private readonly IUnitOfWork _unitOfWork;

            public CreateProductCommandHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<Product> Handle(CreateProductCommand request, CancellationToken cancellationToken)
            {
                var product = new Product(request.Name, request.Price);

                await _unitOfWork.Products.AddAsync(product);
                await _unitOfWork.CompleteAsync();

                return product;
            }
        }
    }
}
