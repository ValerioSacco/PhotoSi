
using MediatR;

namespace PhotoSi.ProductsService.Features.CreateProduct
{
    public record CreateProductCommand(string name, string description, Guid categoryId) : IRequest<Guid>;
  
}
