using MediatR;
using PhotoSi.ProductsService.Repositories;
using PhotoSi.Shared.Exceptions;

namespace PhotoSi.ProductsService.Features.DeleteProduct
{
    public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand>
    {
        private readonly IProductRepository _productRepository;

        public DeleteProductCommandHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
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
                await _productRepository.SaveChangesAsync(cancellationToken);
            }
            else
            {
                throw new Exception("Failed to delete product");
            }
        }
    }
}
