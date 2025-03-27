using Moq;
using NUnit.Framework;
using Validata.Application.Commands.Products;
using Validata.Domain.Entities;
using Validata.Infrastructure.Repositories;

namespace Validata.UnitTests.Commands.Products
{
    [TestFixture]
    public class UpdateProductCommandTests
    {
        private Mock<IUnitOfWork> _mockUnitOfWork;
        private Mock<IProductRepository> _mockProductRepo;
        private UpdateProductCommand.UpdateProductCommandHandler _handler;

        [SetUp]
        public void SetUp()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockProductRepo = new Mock<IProductRepository>();
            _mockUnitOfWork.Setup(u => u.Products).Returns(_mockProductRepo.Object);
            _handler = new UpdateProductCommand.UpdateProductCommandHandler(_mockUnitOfWork.Object);
        }

        [Test]
        public async Task Handle_ReturnsTrue_WhenProductUpdated()
        {
            var product = new Product { Id = 1, Name = "Old", Price = 10 };
            _mockProductRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(product);
            _mockProductRepo.Setup(r => r.UpdateAsync(product)).Returns(Task.CompletedTask);
            _mockUnitOfWork.Setup(u => u.CompleteAsync()).ReturnsAsync(1);

            var command = new UpdateProductCommand(1, "Updated", 99.99m);
            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.That(result, Is.True);
            Assert.That(product.Name, Is.EqualTo("Updated"));
            Assert.That(product.Price, Is.EqualTo(99.99m));
            _mockProductRepo.Verify(r => r.UpdateAsync(product), Times.Once);
            _mockUnitOfWork.Verify(u => u.CompleteAsync(), Times.Once);
        }

        [Test]
        public async Task Handle_ReturnsFalse_WhenProductNotFound()
        {
            _mockProductRepo.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Product)null);

            var result = await _handler.Handle(new UpdateProductCommand(99, "Name", 100), CancellationToken.None);

            Assert.That(result, Is.False);
            _mockProductRepo.Verify(r => r.UpdateAsync(It.IsAny<Product>()), Times.Never);
            _mockUnitOfWork.Verify(u => u.CompleteAsync(), Times.Never);
        }
    }
}
