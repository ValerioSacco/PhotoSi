
using MediatR;

namespace PhotoSi.ProductsService.Features.CreateProduct
{
    public record CreateProductCommand(
        string name, 
        string description,
        decimal price,
        string imageUrl,
        Guid categoryId
    ) : IRequest<Guid>;
  
}
