using MediatR;

namespace PhotoSi.ProductsService.Features.DeleteProduct
{
    public record DeleteProductCommand(Guid id) : IRequest;
}
