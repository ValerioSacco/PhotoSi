using MediatR;

namespace PhotoSi.ProductsService.Features.ListProducts
{
    public record ListProductsQuery(int pageNumber = 1, int pageSize = 10) : IRequest<ListProductsResponse>;
}
