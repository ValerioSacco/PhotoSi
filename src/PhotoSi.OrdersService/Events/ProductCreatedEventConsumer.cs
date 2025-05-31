using MassTransit;
using PhotoSi.OrdersService.Models;
using PhotoSi.OrdersService.Repositories;
using PhotoSi.Shared.Events;

namespace PhotoSi.OrdersService.Events
{
    public class ProductCreatedEventConsumer : IConsumer<ProductCreatedEvent>
    {
        private readonly IProductRepository _productRepository;

        public ProductCreatedEventConsumer(IProductRepository productRepository)
        {
            _productRepository = productRepository;
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
            }
            else
            {
                throw new Exception("Failed to create product in the database.");
            }
        }
    }
}
