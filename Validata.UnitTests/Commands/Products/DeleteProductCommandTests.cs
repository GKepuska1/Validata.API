using Moq;
using NUnit.Framework;
using Validata.Application.Commands.Products;
using Validata.Domain.Entities;
using Validata.Infrastructure.Repositories;

namespace Validata.UnitTests.Commands.Products
{
    [TestFixture]
    public class DeleteProductCommandTests
    {
        private Mock<IUnitOfWork> _mockUnitOfWork;
        private Mock<IProductRepository> _mockProductRepo;
        private DeleteProductCommand.DeleteProductCommandHandler _handler;

        [SetUp]
        public void SetUp()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockProductRepo = new Mock<IProductRepository>();
            _mockUnitOfWork.Setup(u => u.Products).Returns(_mockProductRepo.Object);
            _handler = new DeleteProductCommand.DeleteProductCommandHandler(_mockUnitOfWork.Object);
        }

        [Test]
        public async Task Handle_ReturnsTrue_WhenProductExists()
        {
            var product = new Product { Id = 1 };
            _mockProductRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(product);
            _mockProductRepo.Setup(r => r.DeleteAsync(product)).Returns(Task.CompletedTask);
            _mockUnitOfWork.Setup(u => u.CompleteAsync()).ReturnsAsync(1);

            var result = await _handler.Handle(new DeleteProductCommand(1), CancellationToken.None);

            Assert.That(result, Is.True);
            _mockProductRepo.Verify(r => r.DeleteAsync(product), Times.Once);
            _mockUnitOfWork.Verify(u => u.CompleteAsync(), Times.Once);
        }

        [Test]
        public async Task Handle_ReturnsFalse_WhenProductNotFound()
        {
            _mockProductRepo.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Product)null);

            var result = await _handler.Handle(new DeleteProductCommand(99), CancellationToken.None);

            Assert.That(result, Is.False);
            _mockProductRepo.Verify(r => r.DeleteAsync(It.IsAny<Product>()), Times.Never);
            _mockUnitOfWork.Verify(u => u.CompleteAsync(), Times.Never);
        }
    }
}
