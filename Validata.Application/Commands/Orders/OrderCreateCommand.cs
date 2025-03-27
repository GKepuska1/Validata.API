using MediatR;
using Validata.Domain.Dtos;
using Validata.Domain.Entities;
using Validata.Infrastructure.Repositories;

namespace Validata.Application.Commands.Orders
{
    public class OrderCreateCommand : IRequest<Order>
    {
        public int CustomerId { get; }
        public List<OrderItemRequest> Items { get; }

        public OrderCreateCommand(int customerId, List<OrderItemRequest> items)
        {
            CustomerId = customerId;
            Items = items;
        }

        public class OrderCreateCommandHandler : IRequestHandler<OrderCreateCommand, Order>
        {
            private readonly IUnitOfWork _unitOfWork;

            public OrderCreateCommandHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<Order> Handle(OrderCreateCommand request, CancellationToken cancellationToken)
            {
                var customer = await _unitOfWork.Customers.GetByIdAsync(request.CustomerId);
                if (customer == null)
                {
                    throw new Exception("Customer not found.");
                }

                var orderItems = new List<OrderItem>();
                foreach (var item in request.Items)
                {
                    var product = await _unitOfWork.Products.GetByIdAsync(item.ProductId);
                    if (product == null)
                    {
                        throw new Exception($"Product with ID {item.ProductId} not found.");
                    }
                    orderItems.Add(new OrderItem(product.Price, item.Quantity, product.Id));
                }

                var order = new Order()
                {
                    CustomerId = request.CustomerId,
                    TotalPrice = orderItems.Sum(i => i.Price * i.Quantity)
                };
                await _unitOfWork.Orders.AddAsync(order);
                await _unitOfWork.CompleteAsync();

                for (int i = 0; i < orderItems.Count; i++)
                {
                    orderItems[i].OrderId = order.Id;
                    await _unitOfWork.OrderItems.AddAsync(orderItems[i]);
                }
                await _unitOfWork.CompleteAsync();

                return order;
            }
        }
    }
}
