using Moq;
using NUnit.Framework;
using Validata.Application.Commands.Customers;
using Validata.Domain.Entities;
using Validata.Infrastructure.Repositories;

namespace Validata.UnitTests.Commands.Customers
{
    [TestFixture]
    public class UpdateCustomerCommandTests
    {
        private Mock<IUnitOfWork> _mockUnitOfWork;
        private Mock<ICustomerRepository> _mockCustomerRepo;
        private UpdateCustomerCommand.UpdateCustomerCommandHandler _handler;

        [SetUp]
        public void SetUp()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockCustomerRepo = new Mock<ICustomerRepository>();
            _mockUnitOfWork.Setup(u => u.Customers).Returns(_mockCustomerRepo.Object);
            _handler = new UpdateCustomerCommand.UpdateCustomerCommandHandler(_mockUnitOfWork.Object);
        }

        [Test]
        public async Task Handle_ReturnsTrue_WhenCustomerExists()
        {
            var customer = new Customer("Old", "Name", "Old Address");
            var command = new UpdateCustomerCommand(1, "New", "Name", "New Address");

            _mockCustomerRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(customer);
            _mockCustomerRepo.Setup(r => r.UpdateAsync(It.IsAny<Customer>())).Returns(Task.CompletedTask);
            _mockUnitOfWork.Setup(u => u.CompleteAsync()).ReturnsAsync(1);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.That(result, Is.True);
            Assert.That(customer.FirstName, Is.EqualTo("New"));
            Assert.That(customer.Address, Is.EqualTo("New Address"));
            _mockCustomerRepo.Verify(r => r.UpdateAsync(customer), Times.Once);
            _mockUnitOfWork.Verify(u => u.CompleteAsync(), Times.Once);
        }

        [Test]
        public async Task Handle_ReturnsFalse_WhenCustomerNotFound()
        {
            _mockCustomerRepo.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Customer)null);

            var command = new UpdateCustomerCommand(99, "First", "Last", "Address");
            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.That(result, Is.False);
            _mockCustomerRepo.Verify(r => r.UpdateAsync(It.IsAny<Customer>()), Times.Never);
            _mockUnitOfWork.Verify(u => u.CompleteAsync(), Times.Never);
        }
    }
}
