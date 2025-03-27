using MediatR;
using Validata.Infrastructure.Repositories;

namespace Validata.Application.Commands.Products
{
    public class UpdateProductCommand : IRequest<bool>
    {
        public int Id { get; }
        public string Name { get; }
        public decimal Price { get; }

        public UpdateProductCommand(int id, string name, decimal price)
        {
            Id = id;
            Name = name;
            Price = price;
        }

        public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, bool>
        {
            private readonly IUnitOfWork _unitOfWork;

            public UpdateProductCommandHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<bool> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
            {
                var product = await _unitOfWork.Products.GetByIdAsync(request.Id);
                if (product == null)
                    return false;

                product.Name = request.Name;
                product.Price = request.Price;

                await _unitOfWork.Products.UpdateAsync(product);
                await _unitOfWork.CompleteAsync();

                return true;
            }
        }
    }
}
