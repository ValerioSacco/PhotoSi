using PhotoSi.ProductsService.Features.ListProducts;

namespace PhotoSi.ProductsService.Features.ListCategories
{
    public record CategoryResponse(Guid id, string name, string description);
    public record ListCategoriesResponse(IList<CategoryResponse> categories);
}
