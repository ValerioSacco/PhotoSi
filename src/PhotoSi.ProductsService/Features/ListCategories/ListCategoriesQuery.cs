using MediatR;

namespace PhotoSi.ProductsService.Features.ListCategories
{
    public record ListCategoriesQuery() : IRequest<ListCategoriesResponse>;
}
