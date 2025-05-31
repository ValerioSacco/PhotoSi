using MediatR;
using PhotoSi.OrdersService.Repositories;
using PhotoSi.Shared.Exceptions;

namespace PhotoSi.OrdersService.Features.DeleteOrder
{
    public class DeleteOrderCommandHandler : IRequestHandler<DeleteOrderCommand>
    {
        private readonly IOrderRepository _orderRepository;

        public DeleteOrderCommandHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
        {
            var product = await _orderRepository.GetByIdAsync(request.id, cancellationToken);

            if (product is null)
            {
                throw new NotFoundException("The order requested does not exist.");
            }

            if (_orderRepository.Delete(product))
            {
                await _orderRepository
                    .SaveChangesAsync(cancellationToken);
            }
            else
            {
                throw new Exception("Failed to delete order");
            }
        }
    }
}
