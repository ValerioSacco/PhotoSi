using MassTransit;
using MediatR;
using PhotoSi.ProductsService.Repositories;
using PhotoSi.Shared.Events;
using PhotoSi.Shared.Exceptions;

namespace PhotoSi.ProductsService.Features.UpdateProduct
{
    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, Guid>
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IPublishEndpoint _publishEndpoint;

        public UpdateProductCommandHandler(
            IProductRepository productRepository,
            ICategoryRepository categoryRepository,
            IPublishEndpoint publishEndpoint)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _publishEndpoint = publishEndpoint;
        }

        public async Task<Guid> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var category = await _categoryRepository.GetByIdAsync(request.categoryId, cancellationToken);
            if (category is null)
            {
                throw new BusinessRuleException("A product must be associated to an existing category.");
            }

            var product = await _productRepository.GetByIdAsync(request.id, cancellationToken);

            if (product is null)
            {
                throw new NotFoundException("The product requested does not exist.");
            }

            product.Name = request.name;
            product.Description = request.description;
            product.Price = request.price;
            product.ImageUrl = request.imageUrl;
            product.CategoryId = request.categoryId;

            if (_productRepository.Update(product))
            {
                await _productRepository.SaveChangesAsync(cancellationToken);
                //I should send the event to an outbox before saving the changes
                await _publishEndpoint.Publish(
                    new ProductUpdatedEvent(
                        product.Id,
                        product.Name,
                        product.Description,
                        product.Price,
                        category.Name
                ), cancellationToken);

                return product.Id;
            }
            else
            {
                throw new Exception("Failed to update product");
            }
        }
    }
}
