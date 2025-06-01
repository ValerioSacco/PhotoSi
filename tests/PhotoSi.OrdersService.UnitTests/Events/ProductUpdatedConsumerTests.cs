using MassTransit;
using Microsoft.Extensions.Logging;
using NSubstitute;
using PhotoSi.OrdersService.Events;
using PhotoSi.OrdersService.Models;
using PhotoSi.OrdersService.Repositories;
using PhotoSi.Shared.Events;

namespace PhotoSi.OrdersService.UnitTests.Events
{
    public class ProductUpdatedConsumerTests
    {
        private readonly ProductUpdatedEventConsumer _consumer;
        private readonly IProductRepository _productRepository = Substitute.For<IProductRepository>();
        private readonly ILogger<ProductUpdatedEventConsumer> _logger = Substitute.For<ILogger<ProductUpdatedEventConsumer>>();
        private readonly ConsumeContext<ProductUpdatedEvent> _context = Substitute.For<ConsumeContext<ProductUpdatedEvent>>();

        public ProductUpdatedConsumerTests()
        {
            _consumer = new ProductUpdatedEventConsumer(_productRepository, _logger);
        }

        private ProductUpdatedEvent CreateProductUpdatedEvent(Guid id)
        {
            return new ProductUpdatedEvent
            (
                id,
                "Test Product",
                "Test Description",
                10.5m,
                "Test Category"
            );
        }

        [Fact]
        public async Task Consume_LogsWarning_WhenProductsNotFound()
        {
            // Arrange
            var productEvent = CreateProductUpdatedEvent(Guid.NewGuid());
            _context.Message.Returns(productEvent);
            _context.CancellationToken.Returns(CancellationToken.None);
            _productRepository.GetByIdAsync(productEvent.id, CancellationToken.None)
                .Returns((Product?)null);

            // Act
            await _consumer.Consume(_context);

            // Assert
            _logger.Received(1).LogWarning($"Product not found: {productEvent.id}");
        }

        [Fact]
        public async Task Consume_SavesProduct_WhenProductIsReceivedSuccessfully()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var product = new Product
            {
                Id = productId,
                Name = "Old",
                Description = "OldDesc",
                Price = 1,
                CategoryName = "OldCat"
            };
            var productEvent = CreateProductUpdatedEvent(productId);
            _context.Message.Returns(productEvent);
            _context.CancellationToken.Returns(CancellationToken.None);
            _productRepository.GetByIdAsync(productId, CancellationToken.None)
                .Returns(product);
            _productRepository.Update(product).Returns(true);

            // Act
            await _consumer.Consume(_context);

            // Assert
            _productRepository.Received(1).Update(product);
            await _productRepository.Received(1).SaveChangesAsync(CancellationToken.None);
        }


        [Fact]
        public async Task Consume_LogsInfo_WhenProductIsReceivedSuccessfully()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var product = new Product
            {
                Id = productId,
                Name = "Old",
                Description = "OldDesc",
                Price = 1,
                CategoryName = "OldCat"
            };
            var productEvent = CreateProductUpdatedEvent(productId);
            _context.Message.Returns(productEvent);
            _context.CancellationToken.Returns(CancellationToken.None);
            _productRepository.GetByIdAsync(productId, CancellationToken.None)
                .Returns(product);
            _productRepository.Update(product).Returns(true);

            // Act
            await _consumer.Consume(_context);

            // Assert
            _logger.Received(1).LogInformation($"Product updated successfully: {productId}");
        }

        [Fact]
        public async Task Consume_LogsError_WhenProductUpdateFails()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var product = new Product
            {
                Id = productId,
                Name = "Old",
                Description = "OldDesc",
                Price = 1,
                CategoryName = "OldCat"
            };
            var productEvent = CreateProductUpdatedEvent(productId);
            _context.Message.Returns(productEvent);
            _context.CancellationToken.Returns(CancellationToken.None);
            _productRepository.GetByIdAsync(productId, CancellationToken.None)
                .Returns(product);
            _productRepository.Update(product).Returns(false);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<Exception>(() => _consumer.Consume(_context));
            Assert.Equal("Failed to update product in the database.", ex.Message);
            _logger.Received(1).LogError($"Failed to update product: {productId}");
        }
    }
}
