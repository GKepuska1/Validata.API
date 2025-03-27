using Moq;
using NUnit.Framework;
using Validata.Application.Commands.Customers.Queries;
using Validata.Domain.Entities;
using Validata.Infrastructure.Repositories;

namespace Validata.UnitTests.Commands.Customers.Queries
{
    [TestFixture]
    public class GetAllCustomersQueryTests
    {
        private Mock<IUnitOfWork> _mockUnitOfWork;
        private GetAllCustomersQuery.GetAllCustomersQueryHandler _handler;


        [SetUp]
        public void SetUp()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _handler = new GetAllCustomersQuery.GetAllCustomersQueryHandler(_mockUnitOfWork.Object);
        }

        [Test]
        public async Task Handle_ReturnsAllCustomers()
        {
            var customers = new List<Customer>
            {
                new Customer("Jhon", "Test", "Transit"),
                new Customer("Jhon2", "Test2", "Transit2"),
            };

            _mockUnitOfWork.Setup(u => u.Customers.GetAllAsync()).ReturnsAsync(customers);

            var result = await _handler.Handle(new GetAllCustomersQuery(), CancellationToken.None);

            Assert.That(result, Is.EqualTo(customers));
            _mockUnitOfWork.Verify(u => u.Customers.GetAllAsync(), Times.Once);
        }
    }
}
