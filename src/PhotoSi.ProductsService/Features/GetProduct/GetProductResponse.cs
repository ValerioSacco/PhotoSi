namespace PhotoSi.ProductsService.Features.GetProduct
{
    public record  GetProductResponse(Guid id, string name, string description, string categoryName);
}
