using Moq;
using NUnit.Framework;
using Validata.Application.Commands.Customers.Queries;
using Validata.Domain.Entities;
using Validata.Infrastructure.Repositories;

namespace Validata.UnitTests.Commands.Orders.Queries
{
    [TestFixture]
    public class GetCustomerByIdQueryHandlerTests
    {
        private Mock<IUnitOfWork> _mockUnitOfWork;
        private Mock<ICustomerRepository> _mockCustomerRepo;
        private GetCustomerByIdQuery.GetCustomerByIdQueryHandler _handler;

        [SetUp]
        public void SetUp()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockCustomerRepo = new Mock<ICustomerRepository>();
            _mockUnitOfWork.Setup(u => u.Customers).Returns(_mockCustomerRepo.Object);
            _handler = new GetCustomerByIdQuery.GetCustomerByIdQueryHandler(_mockUnitOfWork.Object);
        }

        [Test]
        public async Task Handle_ReturnsCustomer_WhenExists()
        {
            var customer = new Customer("Test", "test", "test") { Id = 1 };
            _mockCustomerRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(customer);

            var result = await _handler.Handle(new GetCustomerByIdQuery(1), CancellationToken.None);

            Assert.That(result, Is.EqualTo(customer));
            _mockCustomerRepo.Verify(r => r.GetByIdAsync(1), Times.Once);
        }

        [Test]
        public async Task Handle_ReturnsNull_WhenNotFound()
        {
            _mockCustomerRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Customer)null);

            var result = await _handler.Handle(new GetCustomerByIdQuery(1), CancellationToken.None);

            Assert.That(result, Is.Null);
            _mockCustomerRepo.Verify(r => r.GetByIdAsync(1), Times.Once);
        }
    }
}
