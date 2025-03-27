using Moq;
using NUnit.Framework;
using Validata.Application.Commands.Orders.Queries;
using Validata.Domain.Entities;
using Validata.Infrastructure.Repositories;

namespace Validata.UnitTests.Commands.Orders.Queries
{
    [TestFixture]
    public class GetAllOrdersQueryHandlerTests
    {
        private Mock<IUnitOfWork> _mockUnitOfWork;
        private Mock<IOrderRepository> _mockOrderRepo;
        private GetAllOrdersQuery.GetAllOrdersQueryHandler _handler;

        [SetUp]
        public void SetUp()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockOrderRepo = new Mock<IOrderRepository>();
            _mockUnitOfWork.Setup(u => u.Orders).Returns(_mockOrderRepo.Object);
            _handler = new GetAllOrdersQuery.GetAllOrdersQueryHandler(_mockUnitOfWork.Object);
        }

        [Test]
        public async Task Handle_ReturnsAllOrders()
        {
            var orders = new List<Order> { new Order { Id = 1 }, new Order { Id = 2 } };
            _mockOrderRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(orders);

            var result = await _handler.Handle(new GetAllOrdersQuery(), CancellationToken.None);

            Assert.That(result, Is.EqualTo(orders));
            _mockOrderRepo.Verify(r => r.GetAllAsync(), Times.Once);
        }
    }
}
