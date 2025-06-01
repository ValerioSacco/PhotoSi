using MassTransit;
using Microsoft.Extensions.Logging;
using NSubstitute;
using PhotoSi.OrdersService.Events;
using PhotoSi.OrdersService.Models;
using PhotoSi.OrdersService.Repositories;
using PhotoSi.Shared.Events;

namespace PhotoSi.OrdersService.UnitTests.Events
{
    public class ProductDeletedConsumerTests
    {
        private readonly ProductDeletedEventConsumer _consumer;
        private readonly IProductRepository _productRepository = Substitute.For<IProductRepository>();
        private readonly ILogger<ProductDeletedEventConsumer> _logger = Substitute.For<ILogger<ProductDeletedEventConsumer>>();

        public ProductDeletedConsumerTests()
        {
            _consumer = new ProductDeletedEventConsumer(_productRepository, _logger);
        }


        private static ProductDeletedEvent CreateProductDeletedEvent(Guid productId)
        {
            return new ProductDeletedEvent(productId);
        }

        [Fact]
        public async Task Consume_RemovesProduct_WhenProductIsReceivedSuccessfully()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var product = new Product { Id = productId, IsAvailable = true };
            var productEvent = CreateProductDeletedEvent(productId);
            var context = Substitute.For<ConsumeContext<ProductDeletedEvent>>();
            context.Message.Returns(productEvent);
            context.CancellationToken.Returns(CancellationToken.None);

            _productRepository.GetByIdAsync(productId, CancellationToken.None)
                .Returns(product);
            _productRepository.Update(product).Returns(true);

            // Act
            await _consumer.Consume(context);

            // Assert
            Assert.False(product.IsAvailable);
            await _productRepository.Received(1).SaveChangesAsync(CancellationToken.None);
        }

        [Fact]
        public async Task Consume_LogsWarning_WhenProductsNotFound()
        {
            // Arrange
            var productEvent = CreateProductDeletedEvent(Guid.NewGuid());
            var context = Substitute.For<ConsumeContext<ProductDeletedEvent>>();
            context.Message.Returns(productEvent);
            context.CancellationToken.Returns(CancellationToken.None);

            _productRepository.GetByIdAsync(productEvent.id, CancellationToken.None)
                .Returns((Product?)null);

            // Act
            await _consumer.Consume(context);

            // Assert
            _logger.Received(1).LogWarning($"Product not found: {productEvent.id}");
        }

        [Fact]
        public async Task Consume_LogsInfo_WhenProductIsReceivedSuccessfully()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var product = new Product { Id = productId, IsAvailable = true };
            var productEvent = CreateProductDeletedEvent(productId);
            var context = Substitute.For<ConsumeContext<ProductDeletedEvent>>();
            context.Message.Returns(productEvent);
            context.CancellationToken.Returns(CancellationToken.None);

            _productRepository.GetByIdAsync(productId, CancellationToken.None)
                .Returns(product);
            _productRepository.Update(product).Returns(true);

            // Act
            await _consumer.Consume(context);

            // Assert
            Assert.False(product.IsAvailable);
            _logger.Received(1).LogInformation($"Product set as unavailable successfully: {product.Id}");
        }

        [Fact]
        public async Task Consume_LogsError_WhenProductDeletionFails()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var product = new Product { Id = productId, IsAvailable = true };
            var productEvent = CreateProductDeletedEvent(productId);
            var context = Substitute.For<ConsumeContext<ProductDeletedEvent>>();
            context.Message.Returns(productEvent);
            context.CancellationToken.Returns(CancellationToken.None);

            _productRepository.GetByIdAsync(productId, CancellationToken.None)
                .Returns(product);
            _productRepository.Update(product).Returns(false);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<Exception>(() => _consumer.Consume(context));
            Assert.Equal("Failed to set product unavailable in the database.", ex.Message);
            _logger.Received(1).LogError($"Failed to set product unavailble: {product.Id}");
        }
    }
}
