using MediatR;

namespace PhotoSi.ProductsService.Features.UpdateProduct
{
   public record UpdateProductCommand(
       Guid id, 
       string name, 
       string description,
       decimal price,
       string imageUrl,
       Guid categoryId
   ) : IRequest<Guid>;
}
