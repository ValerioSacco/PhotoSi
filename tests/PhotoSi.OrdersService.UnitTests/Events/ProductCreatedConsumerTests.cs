using MassTransit;
using Microsoft.Extensions.Logging;
using NSubstitute;
using PhotoSi.OrdersService.Events;
using PhotoSi.OrdersService.Models;
using PhotoSi.OrdersService.Repositories;
using PhotoSi.Shared.Events;


namespace PhotoSi.OrdersService.UnitTests.Events
{
    public class ProductCreatedConsumerTests
    {
        private readonly ProductCreatedEventConsumer _consumer;
        private readonly IProductRepository _productRepository = Substitute.For<IProductRepository>();
        private readonly ILogger<ProductCreatedEventConsumer> _logger = Substitute.For<ILogger<ProductCreatedEventConsumer>>();

        public ProductCreatedConsumerTests()
        {
            _consumer = new ProductCreatedEventConsumer(_productRepository, _logger);
        }

        private ProductCreatedEvent ProductCreatedEvent()
        {
            return new ProductCreatedEvent(
                Guid.NewGuid(),
                "Test Product",
                "Test Description",
                10.5m,
                "Test Category"
            );

        }

        [Fact]
        public async Task Consume_SavesProduct_WhenProductIsReceivedSuccessfully()
        {
            // Arrange
            var productEvent = ProductCreatedEvent();
            var context = Substitute.For<ConsumeContext<ProductCreatedEvent>>();
            context.Message.Returns(productEvent);
            context.CancellationToken.Returns(CancellationToken.None);

            _productRepository.Create(Arg.Any<Product>()).Returns(true);
            _productRepository.SaveChangesAsync(Arg.Any<CancellationToken>()).Returns(Task.FromResult(1));

            // Act
            await _consumer.Consume(context);

            // Assert
            _productRepository.Received(1).Create(Arg.Is<Product>(p =>
                p.Id == productEvent.id &&
                p.Name == productEvent.name &&
                p.Description == productEvent.description &&
                p.Price == productEvent.price &&
                p.CategoryName == productEvent.categoryName
            ));
            await _productRepository.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
        }


        [Fact]
        public async Task Consume_LogsInfo_WhenProductIsReceivedSuccessfully()
        {
            // Arrange
            var productEvent = ProductCreatedEvent();
            var context = Substitute.For<ConsumeContext<ProductCreatedEvent>>();
            context.Message.Returns(productEvent);
            context.CancellationToken.Returns(CancellationToken.None);

            _productRepository.Create(Arg.Any<Product>()).Returns(true);
            _productRepository.SaveChangesAsync(Arg.Any<CancellationToken>()).Returns(Task.FromResult(1));

            // Act
            await _consumer.Consume(context);

            // Assert
            _productRepository.Received(1).Create(Arg.Is<Product>(p =>
                p.Id == productEvent.id &&
                p.Name == productEvent.name &&
                p.Description == productEvent.description &&
                p.Price == productEvent.price &&
                p.CategoryName == productEvent.categoryName
            ));
            _logger.Received(1).LogInformation($"Product created successfully: {productEvent.id}");
        }


        [Fact]
        public async Task Consume_LogsError_WhenProductCreationFails()
        {
            // Arrange
            var productEvent = ProductCreatedEvent();
            var context = Substitute.For<ConsumeContext<ProductCreatedEvent>>();
            context.Message.Returns(productEvent);
            context.CancellationToken.Returns(CancellationToken.None);

            _productRepository.Create(Arg.Any<Product>()).Returns(false);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<Exception>(() => _consumer.Consume(context));
            _logger.Received(1).LogError($"Failed to create product: {productEvent.id}");
        }
    }
}
