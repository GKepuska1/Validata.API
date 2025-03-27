using Moq;
using NUnit.Framework;
using Validata.Application.Commands.Orders;
using Validata.Domain.Dtos;
using Validata.Domain.Entities;
using Validata.Infrastructure.Repositories;

namespace Validata.UnitTests.Commands.Orders
{
    [TestFixture]
    public class UpdateOrderCommandTests
    {
        private Mock<IUnitOfWork> _mockUnitOfWork;
        private Mock<IOrderRepository> _mockOrderRepo;
        private Mock<IProductRepository> _mockProductRepo;
        private Mock<ICustomerRepository> _mockCustomerRepo;
        private Mock<IOrderItemRepository> _mockOrderItemRepo;
        private UpdateOrderCommand.UpdateOrderCommandHandler _handler;

        [SetUp]
        public void SetUp()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockOrderRepo = new Mock<IOrderRepository>();
            _mockProductRepo = new Mock<IProductRepository>();
            _mockCustomerRepo = new Mock<ICustomerRepository>();
            _mockOrderItemRepo = new Mock<IOrderItemRepository>();

            _mockUnitOfWork.Setup(u => u.Orders).Returns(_mockOrderRepo.Object);
            _mockUnitOfWork.Setup(u => u.Products).Returns(_mockProductRepo.Object);
            _mockUnitOfWork.Setup(u => u.Customers).Returns(_mockCustomerRepo.Object);
            _mockUnitOfWork.Setup(u => u.OrderItems).Returns(_mockOrderItemRepo.Object);

            _handler = new UpdateOrderCommand.UpdateOrderCommandHandler(_mockUnitOfWork.Object);
        }

        [Test]
        public async Task Handle_UpdatesOrderSuccessfully()
        {
            var existingOrder = new Order { Id = 1, CustomerId = 1, OrderItems = new List<OrderItem>() };
            var customer = new Customer("test", "test", "test") { Id = 2 };
            var product = new Product { Id = 10, Price = 50 };

            var itemReq = new List<OrderItemRequest>
            {
                new OrderItemRequest { ProductId = 10, Quantity = 3 }
            };

            _mockOrderRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(existingOrder);
            _mockCustomerRepo.Setup(r => r.GetByIdAsync(2)).ReturnsAsync(customer);
            _mockProductRepo.Setup(r => r.GetByIdAsync(10)).ReturnsAsync(product);
            _mockOrderItemRepo.Setup(r => r.GetByOrderId(1)).ReturnsAsync(new List<OrderItem>());

            _mockOrderRepo.Setup(r => r.UpdateAsync(It.IsAny<Order>())).Returns(Task.CompletedTask);
            _mockUnitOfWork.Setup(u => u.CompleteAsync()).ReturnsAsync(1);

            var command = new UpdateOrderCommand(1, 2, itemReq);
            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.That(result.CustomerId, Is.EqualTo(2));
            Assert.That(result.OrderItems.Count, Is.EqualTo(1));
            Assert.That(result.OrderItems.First().ProductId, Is.EqualTo(10));
            Assert.That(result.OrderItems.First().Quantity, Is.EqualTo(3));

            _mockOrderRepo.Verify(r => r.UpdateAsync(existingOrder), Times.Once);
            _mockUnitOfWork.Verify(u => u.CompleteAsync(), Times.Once);
        }

        [Test]
        public void Handle_ThrowsException_WhenOrderNotFound()
        {
            _mockOrderRepo.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Order)null);
            var command = new UpdateOrderCommand(1, 1, new List<OrderItemRequest>());

            Assert.ThrowsAsync<Exception>(() => _handler.Handle(command, CancellationToken.None), "Order not found.");
        }

        [Test]
        public void Handle_ThrowsException_WhenCustomerNotFound()
        {
            _mockOrderRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(new Order { Id = 1 });
            _mockOrderItemRepo.Setup(r => r.GetByOrderId(1)).ReturnsAsync(new List<OrderItem>());
            _mockCustomerRepo.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Customer)null);

            var command = new UpdateOrderCommand(1, 999, new List<OrderItemRequest>());

            Assert.ThrowsAsync<Exception>(() => _handler.Handle(command, CancellationToken.None), "Customer not found.");
        }

        [Test]
        public void Handle_ThrowsException_WhenProductNotFound()
        {
            _mockOrderRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(new Order { Id = 1 });
            _mockOrderItemRepo.Setup(r => r.GetByOrderId(1)).ReturnsAsync(new List<OrderItem>());
            _mockCustomerRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(new Customer("test", "test", "test") { Id = 1 });
            _mockProductRepo.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Product)null);

            var items = new List<OrderItemRequest> { new OrderItemRequest { ProductId = 404, Quantity = 1 } };
            var command = new UpdateOrderCommand(1, 1, items);

            Assert.ThrowsAsync<Exception>(() => _handler.Handle(command, CancellationToken.None), "Product with ID 404 not found.");
        }
    }
}
