using Moq;
using NUnit.Framework;
using Validata.Application.Commands.Customers.Queries;
using Validata.Domain.Entities;
using Validata.Infrastructure.Repositories;

namespace Validata.UnitTests.Commands.Customers.Queries
{
    [TestFixture]
    public class GetCustomerByIdQueryTests
    {
        private Mock<IUnitOfWork> _mockUnitOfWork;
        private GetCustomerByIdQuery.GetCustomerByIdQueryHandler _handler;

        [SetUp]
        public void SetUp()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _handler = new GetCustomerByIdQuery.GetCustomerByIdQueryHandler(_mockUnitOfWork.Object);
        }

        [Test]
        public async Task Handle_ReturnsCustomer_WhenIdExists()
        {
            var customer = new Customer("Test", "LastTest", "Address");

            _mockUnitOfWork.Setup(u => u.Customers.GetByIdAsync(1))
                .ReturnsAsync(customer);

            var query = new GetCustomerByIdQuery(1);
            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.That(result, Is.EqualTo(customer));
            _mockUnitOfWork.Verify(u => u.Customers.GetByIdAsync(1), Times.Once);
        }
    }
}
