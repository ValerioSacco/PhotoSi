
using MediatR;

namespace PhotoSi.ProductsService.Features.CreateProduct
{
    public record CreateProductCommand(string name, string description, string categoryName) : IRequest<Guid>;
  
}
