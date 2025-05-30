namespace PhotoSi.ProductsService.Features.ListProducts
{
    public record ProductResponse(
        Guid id, 
        string name, 
        string description,
        decimal price,
        string imageUrl,
        string categoryName     
    );
    
    public record ListProductsResponse(
        int count, 
        int pageNumber, 
        int pageSize, 
        IList<ProductResponse> products
    );
}
