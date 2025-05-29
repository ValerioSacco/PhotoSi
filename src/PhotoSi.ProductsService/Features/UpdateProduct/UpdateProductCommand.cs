using MediatR;

namespace PhotoSi.ProductsService.Features.UpdateProduct
{
   public record UpdateProductCommand(Guid id, string name, string description, Guid categoryId) : IRequest<Guid>;
}
