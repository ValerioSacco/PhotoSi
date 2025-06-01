using MassTransit;
using PhotoSi.OrdersService.Repositories;
using PhotoSi.Shared.Events;

namespace PhotoSi.OrdersService.Events
{
    public class ProductUpdatedEventConsumer : IConsumer<ProductUpdatedEvent>
    {
        private readonly IProductRepository _productRepository;
        private readonly ILogger<ProductUpdatedEventConsumer> _logger;

        public ProductUpdatedEventConsumer(
            IProductRepository productRepository,
            ILogger<ProductUpdatedEventConsumer> logger
        )
        {
            _productRepository = productRepository;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<ProductUpdatedEvent> context)
        {
            var product = await _productRepository
                .GetByIdAsync(context.Message.id, context.CancellationToken);

            if (product is null)
            {
                _logger.LogWarning($"Product not found: {context.Message.id}");
                return;
            }

            product.Name = context.Message.name;
            product.Description = context.Message.description;
            product.Price = context.Message.price;
            product.CategoryName = context.Message.categoryName;
            
            var updated = _productRepository.Update(product);

            if (updated)
            {
                await _productRepository
                    .SaveChangesAsync(context.CancellationToken);
                _logger.LogInformation($"Product updated successfully: {product.Id}");
            }
            else
            {
                _logger.LogError($"Failed to update product: {product.Id}");
                throw new Exception("Failed to update product in the database.");
            }
        }
    }
}
