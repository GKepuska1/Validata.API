using Moq;
using NUnit.Framework;
using Validata.Application.Commands.Products.Queries;
using Validata.Domain.Entities;
using Validata.Infrastructure.Repositories;

namespace Validata.UnitTests.Commands.Products.Queries
{
    [TestFixture]
    public class GetAllProductsQueryTests
    {
        private Mock<IUnitOfWork> _mockUnitOfWork;
        private Mock<IProductRepository> _mockProductRepo;
        private GetAllProductsQuery.GetAllProductsQueryHandler _handler;

        [SetUp]
        public void SetUp()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockProductRepo = new Mock<IProductRepository>();
            _mockUnitOfWork.Setup(u => u.Products).Returns(_mockProductRepo.Object);
            _handler = new GetAllProductsQuery.GetAllProductsQueryHandler(_mockUnitOfWork.Object);
        }

        [Test]
        public async Task Handle_ReturnsAllProducts()
        {
            var products = new List<Product> { new Product { Id = 1 }, new Product { Id = 2 } };
            _mockProductRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(products);

            var result = await _handler.Handle(new GetAllProductsQuery(), CancellationToken.None);

            Assert.That(result, Is.EqualTo(products));
            _mockProductRepo.Verify(r => r.GetAllAsync(), Times.Once);
        }
    }
}
