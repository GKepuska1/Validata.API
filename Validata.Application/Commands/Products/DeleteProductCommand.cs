using MediatR;
using Validata.Infrastructure.Repositories;

namespace Validata.Application.Commands.Products
{
    public class DeleteProductCommand : IRequest<bool>
    {
        public int Id { get; }

        public DeleteProductCommand(int id)
        {
            Id = id;
        }

        public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, bool>
        {
            private readonly IUnitOfWork _unitOfWork;

            public DeleteProductCommandHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<bool> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
            {
                var product = await _unitOfWork.Products.GetByIdAsync(request.Id);
                if (product == null)
                    return false;

                await _unitOfWork.Products.DeleteAsync(product);
                await _unitOfWork.CompleteAsync();

                return true;
            }
        }
    }
}
