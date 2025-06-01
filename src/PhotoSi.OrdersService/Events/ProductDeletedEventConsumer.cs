using MassTransit;
using PhotoSi.OrdersService.Repositories;
using PhotoSi.Shared.Events;

namespace PhotoSi.OrdersService.Events
{
    public class ProductDeletedEventConsumer : IConsumer<ProductDeletedEvent>
    {
        private readonly IProductRepository _productRepository;
        private readonly ILogger<ProductDeletedEventConsumer> _logger;

        public ProductDeletedEventConsumer(
            IProductRepository productRepository, 
            ILogger<ProductDeletedEventConsumer> logger
        )
        {
            _productRepository = productRepository;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<ProductDeletedEvent> context)
        {
            var product = await _productRepository
                .GetByIdAsync(context.Message.id, context.CancellationToken);

            if (product is null) {
                _logger.LogWarning($"Product not found: {context.Message.id}");
                return;
            }

            product.IsAvailable = false;
            var updated = _productRepository.Update(product);

            if (updated)
            {
                await _productRepository
                    .SaveChangesAsync(context.CancellationToken);
                _logger.LogInformation($"Product set as unavailable successfully: {product.Id}");
            }
            else
            {
                _logger.LogError($"Failed to set product unavailble: {product.Id}");
                throw new Exception("Failed to set product unavailable in the database.");
            }
        }

    }
}
