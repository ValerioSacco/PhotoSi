using MediatR;
using PhotoSi.ProductsService.Repositories;
using PhotoSi.Shared.Exceptions;

namespace PhotoSi.ProductsService.Features.GetProduct
{
    public class GetProductQueryHandler : IRequestHandler<GetProductQuery, GetProductResponse>
    {
        private readonly IProductRepository _productRespository;

        public GetProductQueryHandler(IProductRepository productRespository)
        {
            _productRespository = productRespository;
        }

        public async Task<GetProductResponse> Handle(
            GetProductQuery request, 
            CancellationToken cancellationToken
        )
        {
            var product = await _productRespository
                .GetByIdAsync(request.id, cancellationToken);

            if (product is null) {
                throw new NotFoundException("The product requested does not exist.");
            }

            return new GetProductResponse(
                product.Id,
                product.Name,
                product.Description,
                product.Price,
                product.ImageUrl,
                product.Category.Name
            );
        }
    }
}
