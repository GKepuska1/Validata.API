using Moq;
using NUnit.Framework;
using Validata.Application.Commands.Products;
using Validata.Domain.Entities;
using Validata.Infrastructure.Repositories;

namespace Validata.UnitTests.Commands.Products
{
    [TestFixture]
    public class CreateProductCommandTests
    {
        private Mock<IUnitOfWork> _mockUnitOfWork;
        private Mock<IProductRepository> _mockProductRepo;
        private CreateProductCommand.CreateProductCommandHandler _handler;

        [SetUp]
        public void SetUp()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockProductRepo = new Mock<IProductRepository>();
            _mockUnitOfWork.Setup(u => u.Products).Returns(_mockProductRepo.Object);
            _handler = new CreateProductCommand.CreateProductCommandHandler(_mockUnitOfWork.Object);
        }

        [Test]
        public async Task Handle_CreatesAndReturnsProduct()
        {
            Product addedProduct = null;

            _mockProductRepo.Setup(r => r.AddAsync(It.IsAny<Product>()))
                .Callback<Product>(p => addedProduct = p)
                .Returns(Task.CompletedTask);

            _mockUnitOfWork.Setup(u => u.CompleteAsync()).ReturnsAsync(1);

            var command = new CreateProductCommand("Test Product", 99.99m);
            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.That(result.Name, Is.EqualTo("Test Product"));
            Assert.That(result.Price, Is.EqualTo(99.99m));
            Assert.That(result, Is.EqualTo(addedProduct));
            _mockProductRepo.Verify(r => r.AddAsync(It.IsAny<Product>()), Times.Once);
            _mockUnitOfWork.Verify(u => u.CompleteAsync(), Times.Once);
        }
    }
}
