using MediatR;
using PhotoSi.ProductsService.Exceptions;
using PhotoSi.ProductsService.Repositories;

namespace PhotoSi.ProductsService.Features.UpdateProduct
{
    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, Guid>
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;

        public UpdateProductCommandHandler(
            IProductRepository productRepository, 
            ICategoryRepository categoryRepository
        )
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
        }

        public async Task<Guid> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            if (await _categoryRepository.GetByIdAsync(request.categoryId, cancellationToken) is null)
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

            _productRepository.Update(product);

            await _productRepository
                .SaveChangesAsync(cancellationToken);

            return product.Id;
        }
    }
}
