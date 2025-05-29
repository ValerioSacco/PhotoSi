namespace PhotoSi.ProductsService.Features.ListProducts
{
    public record ProductResponse(Guid id, string name, string description, string categoryName);
    public record ListProductsResponse(int count, int pageNumber, int pageSize, ICollection<ProductResponse> products);
}
