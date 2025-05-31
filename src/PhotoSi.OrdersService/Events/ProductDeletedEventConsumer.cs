using MassTransit;
using PhotoSi.OrdersService.Repositories;
using PhotoSi.Shared.Events;

namespace PhotoSi.OrdersService.Events
{
    public class ProductDeletedEventConsumer : IConsumer<ProductDeletedEvent>
    {
        private readonly IProductRepository _productRepository;

        public ProductDeletedEventConsumer(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task Consume(ConsumeContext<ProductDeletedEvent> context)
        {
            var product = await _productRepository
                .GetByIdAsync(context.Message.id, context.CancellationToken);

            if (product == null) {
                return;
            }

            product.IsAvailable = false;
            var updated = _productRepository.Update(product);

            if (updated)
            {
                await _productRepository
                    .SaveChangesAsync(context.CancellationToken);
            }
            else
            {
                throw new Exception("Failed to set product unavailable in the database.");
            }
        }

    }
}
