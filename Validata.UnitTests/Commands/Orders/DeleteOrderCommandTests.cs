using Moq;
using NUnit.Framework;
using Validata.Application.Commands.Orders;
using Validata.Domain.Entities;
using Validata.Infrastructure.Repositories;

namespace Validata.UnitTests.Commands.Orders
{
    [TestFixture]
    public class DeleteOrderCommandHandlerTests
    {
        private Mock<IUnitOfWork> _mockUnitOfWork;
        private Mock<IOrderRepository> _mockOrderRepo;
        private DeleteOrderCommand.DeleteOrderCommandHandler _handler;

        [SetUp]
        public void SetUp()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockOrderRepo = new Mock<IOrderRepository>();
            _mockUnitOfWork.Setup(u => u.Orders).Returns(_mockOrderRepo.Object);
            _handler = new DeleteOrderCommand.DeleteOrderCommandHandler(_mockUnitOfWork.Object);
        }

        [Test]
        public async Task Handle_ReturnsTrue_WhenOrderExists()
        {
            var order = new Order { Id = 1 };
            _mockOrderRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(order);
            _mockOrderRepo.Setup(r => r.DeleteAsync(order)).Returns(Task.CompletedTask);
            _mockUnitOfWork.Setup(u => u.CompleteAsync()).ReturnsAsync(1);

            var result = await _handler.Handle(new DeleteOrderCommand(1), CancellationToken.None);

            Assert.That(result, Is.True);
            _mockOrderRepo.Verify(r => r.DeleteAsync(order), Times.Once);
            _mockUnitOfWork.Verify(u => u.CompleteAsync(), Times.Once);
        }

        [Test]
        public async Task Handle_ReturnsFalse_WhenOrderNotFound()
        {
            _mockOrderRepo.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Order)null);

            var result = await _handler.Handle(new DeleteOrderCommand(99), CancellationToken.None);

            Assert.That(result, Is.False);
            _mockOrderRepo.Verify(r => r.DeleteAsync(It.IsAny<Order>()), Times.Never);
            _mockUnitOfWork.Verify(u => u.CompleteAsync(), Times.Never);
        }
    }
}
