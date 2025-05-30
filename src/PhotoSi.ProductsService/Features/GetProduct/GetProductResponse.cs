namespace PhotoSi.ProductsService.Features.GetProduct
{
    public record  GetProductResponse(
        Guid id, 
        string name, 
        string description,
        decimal price,
        string imageUrl,
        string categoryName
    );
}
