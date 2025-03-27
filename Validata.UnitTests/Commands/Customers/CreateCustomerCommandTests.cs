using Moq;
using NUnit.Framework;
using Validata.Application.Commands.Customers;
using Validata.Domain.Entities;
using Validata.Infrastructure.Repositories;

namespace Validata.UnitTests.Commands.Customers
{
    [TestFixture]
    public class CreateCustomerCommandTests
    {
        private Mock<IUnitOfWork> _mockUnitOfWork;
        private Mock<ICustomerRepository> _mockCustomerRepo;
        private CreateCustomerCommand.CreateCustomerCommandHandler _handler;

        [SetUp]
        public void SetUp()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockCustomerRepo = new Mock<ICustomerRepository>();

            _mockUnitOfWork.Setup(u => u.Customers).Returns(_mockCustomerRepo.Object);

            _handler = new CreateCustomerCommand.CreateCustomerCommandHandler(_mockUnitOfWork.Object);
        }

        [Test]
        public async Task Handle_CreatesCustomerSuccessfully()
        {
            var command = new CreateCustomerCommand("John", "Doe", "123 Street");

            Customer createdCustomer = null;
            _mockCustomerRepo.Setup(r => r.AddAsync(It.IsAny<Customer>()))
                .Callback<Customer>(c => createdCustomer = c)
                .Returns(Task.CompletedTask);

            _mockUnitOfWork.Setup(u => u.CompleteAsync()).ReturnsAsync(1);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.FirstName, Is.EqualTo("John"));
            Assert.That(result.LastName, Is.EqualTo("Doe"));
            Assert.That(result.Address, Is.EqualTo("123 Street"));
            _mockCustomerRepo.Verify(r => r.AddAsync(It.IsAny<Customer>()), Times.Once);
            _mockUnitOfWork.Verify(u => u.CompleteAsync(), Times.Once);
        }
    }
}
