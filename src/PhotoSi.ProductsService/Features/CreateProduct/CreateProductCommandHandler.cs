using MediatR;
using PhotoSi.ProductsService.Models;
using PhotoSi.ProductsService.Repositories;

namespace PhotoSi.ProductsService.Features.CreateProduct
{
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Guid>
    {
        private readonly IProductRepository _productRepository;

        public CreateProductCommandHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<Guid> Handle(
            CreateProductCommand request, 
            CancellationToken cancellationToken
        )
        {
            var product = new Product()
            {
                Id = Guid.NewGuid(),
                Name = request.name,
                Description = request.description,
                CategoryId = request.categoryId
            };

            await _productRepository
                .CreateAsync(product, cancellationToken);

            await _productRepository
                .SaveChangesAsync(cancellationToken);
                
            return product.Id;
        }
    }
}
