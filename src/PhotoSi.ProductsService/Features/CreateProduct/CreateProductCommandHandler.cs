using MediatR;
using PhotoSi.ProductsService.Exceptions;
using PhotoSi.ProductsService.Models;
using PhotoSi.ProductsService.Repositories;

namespace PhotoSi.ProductsService.Features.CreateProduct
{
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Guid>
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;

        public CreateProductCommandHandler(
            IProductRepository productRepository, 
            ICategoryRepository categoryRepository
        )
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
        }

        public async Task<Guid> Handle(
            CreateProductCommand request, 
            CancellationToken cancellationToken
        )
        {
            if(await _categoryRepository.GetByIdAsync(request.categoryId, cancellationToken) is null)
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

            _productRepository.Create(product);

            await _productRepository
                .SaveChangesAsync(cancellationToken);
                
            return product.Id;
        }
    }
}
