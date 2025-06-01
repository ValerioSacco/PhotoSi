using MassTransit;
using MediatR;
using PhotoSi.ProductsService.Models;
using PhotoSi.ProductsService.Repositories;
using PhotoSi.Shared.Events;
using PhotoSi.Shared.Exceptions;

namespace PhotoSi.ProductsService.Features.CreateProduct
{
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Guid>
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IPublishEndpoint _publishEndpoint;

        public CreateProductCommandHandler(
            IProductRepository productRepository,
            ICategoryRepository categoryRepository,
            IPublishEndpoint publishEndpoint)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _publishEndpoint = publishEndpoint;
        }

        public async Task<Guid> Handle(
            CreateProductCommand request, 
            CancellationToken cancellationToken
        )
        {
            var category = await _categoryRepository.GetByIdAsync(request.categoryId, cancellationToken);
            if (category  is null)
            {
                throw new BusinessRuleException("A product must be associated to an existing category.");
            }

            var product = new Product()
            {
                Id = Guid.NewGuid(),
                Name = request.name,
                Description = request.description,
                Price = request.price,
                ImageUrl = request.imageUrl,
                CategoryId = request.categoryId
            };

            if (_productRepository.Create(product))
            {
                await _productRepository
                    .SaveChangesAsync(cancellationToken);
                //I should send the event to an outbox before saving the changes
                await _publishEndpoint.Publish(
                    new ProductCreatedEvent(
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
                throw new Exception("Failed to create product");
            }
        }
    }
}
