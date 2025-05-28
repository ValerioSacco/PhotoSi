using MediatR;
using Microsoft.EntityFrameworkCore;
using PhotoSi.ProductsService.Database;
using PhotoSi.ProductsService.Repositories;

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

            return new GetProductResponse(
                product.Id,
                product.Name,
                product.Description,
                product.Category.Name
            );
        }
    }
}
