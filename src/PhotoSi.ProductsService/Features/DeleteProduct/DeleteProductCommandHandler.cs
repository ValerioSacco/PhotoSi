using MassTransit;
using MediatR;
using PhotoSi.ProductsService.Models;
using PhotoSi.ProductsService.Repositories;
using PhotoSi.Shared.Events;
using PhotoSi.Shared.Exceptions;

namespace PhotoSi.ProductsService.Features.DeleteProduct
{
    public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand>
    {
        private readonly IProductRepository _productRepository;
        private readonly IPublishEndpoint _publishEndpoint;

        public DeleteProductCommandHandler(
            IProductRepository productRepository, 
            IPublishEndpoint publishEndpoint
        )
        {
            _productRepository = productRepository;
            _publishEndpoint = publishEndpoint;
        }

        public async Task Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetByIdAsync(request.id, cancellationToken);

            if (product is null)
            {
                throw new NotFoundException("The product requested does not exist.");
            }

            if (_productRepository.Delete(product))
            {
                //I should send the event to an outbox
                await _publishEndpoint.Publish(
                    new ProductDeletedEvent(product.Id), 
                    cancellationToken
                );

                await _productRepository
                    .SaveChangesAsync(cancellationToken);
            }
            else
            {
                throw new Exception("Failed to delete product");
            }
        }
    }
}
