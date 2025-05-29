using MediatR;

namespace PhotoSi.ProductsService.Features.ListProducts
{
    public record ListProductsQuery(int pageNumber, int pageSize) : IRequest<ListProductsResponse>;
}
