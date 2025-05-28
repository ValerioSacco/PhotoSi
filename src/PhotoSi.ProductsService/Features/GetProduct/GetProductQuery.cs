using MediatR;

namespace PhotoSi.ProductsService.Features.GetProduct
{
    public record GetProductQuery(Guid id) : IRequest<GetProductResponse>;
}
  
