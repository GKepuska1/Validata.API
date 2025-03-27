using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Validata.API.Controllers;
using Validata.Domain.Entities;
using Validata.Infrastructure.Repositories;

namespace Validata.UnitTests.Commands.Customers
{
    [TestFixture]
    public class DeleteCustomerCommandTests
    {
        private Mock<IUnitOfWork> _mockUnitOfWork;
        private Mock<ICustomerRepository> _mockCustomerRepo;
        private DeleteCustomerCommand.DeleteCustomerCommandHandler _handler;

        [SetUp]
        public void SetUp()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockCustomerRepo = new Mock<ICustomerRepository>();
            _mockUnitOfWork.Setup(u => u.Customers).Returns(_mockCustomerRepo.Object);
            _handler = new DeleteCustomerCommand.DeleteCustomerCommandHandler(_mockUnitOfWork.Object);
        }

        [Test]
        public async Task Handle_ReturnsTrue_WhenCustomerExists()
        {
            var customer = new Customer("Test", "Test", "Test") { Id = 1 };
            _mockCustomerRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(customer);
            _mockCustomerRepo.Setup(r => r.DeleteAsync(customer)).Returns(Task.CompletedTask);
            _mockUnitOfWork.Setup(u => u.CompleteAsync()).ReturnsAsync(1);

            var result = await _handler.Handle(new DeleteCustomerCommand(1), CancellationToken.None);

            Assert.That(result, Is.True);
            _mockCustomerRepo.Verify(r => r.DeleteAsync(customer), Times.Once);
            _mockUnitOfWork.Verify(u => u.CompleteAsync(), Times.Once);
        }

        [Test]
        public async Task Handle_ReturnsFalse_WhenCustomerNotFound()
        {
            _mockCustomerRepo.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Customer)null);

            var result = await _handler.Handle(new DeleteCustomerCommand(99), CancellationToken.None);

            Assert.That(result, Is.False);
            _mockCustomerRepo.Verify(r => r.DeleteAsync(It.IsAny<Customer>()), Times.Never);
            _mockUnitOfWork.Verify(u => u.CompleteAsync(), Times.Never);
        }
    }
}
