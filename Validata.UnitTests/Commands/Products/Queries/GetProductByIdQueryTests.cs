using Moq;
using NUnit.Framework;
using Validata.Application.Commands.Products.Queries;
using Validata.Domain.Entities;
using Validata.Infrastructure.Repositories;

namespace Validata.UnitTests.Commands.Products.Queries
{
    [TestFixture]
    public class GetProductByIdQueryHandlerTests
    {
        private Mock<IUnitOfWork> _mockUnitOfWork;
        private Mock<IProductRepository> _mockProductRepo;
        private GetProductByIdQuery.GetProductByIdQueryHandler _handler;

        [SetUp]
        public void SetUp()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockProductRepo = new Mock<IProductRepository>();
            _mockUnitOfWork.Setup(u => u.Products).Returns(_mockProductRepo.Object);
            _handler = new GetProductByIdQuery.GetProductByIdQueryHandler(_mockUnitOfWork.Object);
        }

        [Test]
        public async Task Handle_ReturnsProduct_WhenExists()
        {
            var product = new Product { Id = 1 };
            _mockProductRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(product);

            var result = await _handler.Handle(new GetProductByIdQuery(1), CancellationToken.None);

            Assert.That(result, Is.EqualTo(product));
            _mockProductRepo.Verify(r => r.GetByIdAsync(1), Times.Once);
        }

        [Test]
        public async Task Handle_ReturnsNull_WhenNotFound()
        {
            _mockProductRepo.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Product)null);

            var result = await _handler.Handle(new GetProductByIdQuery(99), CancellationToken.None);

            Assert.That(result, Is.Null);
            _mockProductRepo.Verify(r => r.GetByIdAsync(99), Times.Once);
        }
    }
}
