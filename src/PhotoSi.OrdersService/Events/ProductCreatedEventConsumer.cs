using MassTransit;
using PhotoSi.OrdersService.Models;
using PhotoSi.OrdersService.Repositories;
using PhotoSi.Shared.Events;

namespace PhotoSi.OrdersService.Events
{
    public class ProductCreatedEventConsumer : IConsumer<ProductCreatedEvent>
    {
        private readonly IProductRepository _productRepository;
        private readonly ILogger<ProductCreatedEventConsumer> _logger;

        public ProductCreatedEventConsumer(
            IProductRepository productRepository, 
            ILogger<ProductCreatedEventConsumer> logger
        )
        {
            _productRepository = productRepository;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<ProductCreatedEvent> context)
        {
            var product = new Product
            {
                Id = context.Message.id,
                Name = context.Message.name,
                Description = context.Message.description,
                Price = context.Message.price,
                CategoryName = context.Message.categoryName,
            };

            var created = _productRepository.Create(product);

            if (created)
            {
                await _productRepository
                    .SaveChangesAsync(context.CancellationToken);
                _logger.LogInformation($"Product created successfully: {product.Id}");
            }
            else
            {
                _logger.LogError($"Failed to create product: {product.Id}");
                throw new Exception("Failed to create product in the database.");
            }
        }
    }
}
