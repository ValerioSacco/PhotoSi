using MediatR;
using PhotoSi.ProductsService.Repositories;

namespace PhotoSi.ProductsService.Features.ListProducts
{
    public class ListProductsQueryHandler : IRequestHandler<ListProductsQuery, ListProductsResponse>
    {
        private readonly IProductRepository _productRespository;

        public ListProductsQueryHandler(IProductRepository productRespository)
        {
            _productRespository = productRespository;
        }

        public async Task<ListProductsResponse> Handle(
            ListProductsQuery request, 
            CancellationToken cancellationToken
        )
        {
            var totalCount = await _productRespository
                .CountAsync(cancellationToken);

            var products = await _productRespository
                .ListAllAsync(request.pageNumber, request.pageSize, cancellationToken);

            return new ListProductsResponse
            (
                totalCount,
                request.pageNumber,
                request.pageSize,
                products.Select(p => new ProductResponse(
                    p.Id,
                    p.Name,
                    p.Description,
                    p.Price,
                    p.ImageUrl,
                    p.Category?.Name ?? string.Empty
                )).ToList()
            );

        }
    }
}
