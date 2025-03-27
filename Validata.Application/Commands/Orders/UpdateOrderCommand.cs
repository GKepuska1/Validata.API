using MediatR;
using Validata.Domain.Dtos;
using Validata.Domain.Entities;
using Validata.Infrastructure.Repositories;

namespace Validata.Application.Commands.Orders
{
    public class UpdateOrderCommand : IRequest<Order>
    {
        public int Id { get; }
        public int CustomerId { get; }
        public List<OrderItemRequest> Items { get; }

        public UpdateOrderCommand(int id, int customerId, List<OrderItemRequest> items)
        {
            Id = id;
            CustomerId = customerId;
            Items = items ?? new List<OrderItemRequest>();
        }

        public class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand, Order>
        {
            private readonly IUnitOfWork _unitOfWork;

            public UpdateOrderCommandHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<Order> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
            {
                var order = await _unitOfWork.Orders.GetByIdAsync(request.Id);

                if (order == null)
                {
                    throw new Exception("Order not found.");
                }

                var customer = await _unitOfWork.Customers.GetByIdAsync(request.CustomerId);
                if (customer == null)
                {
                    throw new Exception("Customer not found.");
                }

                // Validate the products
                foreach (var item in request.Items)
                {
                    var product = await _unitOfWork.Products.GetByIdAsync(item.ProductId);
                    if (product == null)
                    {
                        throw new Exception($"Product with ID {item.ProductId} not found.");
                    }
                }

                foreach (var itemRequest in request.Items)
                {
                    var existingItem = order.Items.FirstOrDefault(i => i.ProductId == itemRequest.ProductId);

                    if (existingItem != null)
                    {
                        existingItem.Quantity = itemRequest.Quantity;
                    }
                    else
                    {
                        order.Items.Add(new OrderItem(itemRequest.Price, itemRequest.Quantity, itemRequest.ProductId));
                    }
                }

                var requestProductIds = request.Items.Select(i => i.ProductId).ToList();
                var itemsToRemove = order.Items.Where(i => !requestProductIds.Contains(i.ProductId)).ToList();

                foreach (var itemToRemove in itemsToRemove)
                {
                    order.Items.Remove(itemToRemove);
                }


                order.CustomerId = request.CustomerId;
                order.OrderDate = DateTime.Now;

                await _unitOfWork.Orders.UpdateAsync(order);
                await _unitOfWork.CompleteAsync();

                return order;
            }
        }

    }
}
