using MediatR;
using PhotoSi.ProductsService.Database;

namespace PhotoSi.ProductsService.Features.Shared
{
    public class CommandTransactionBehavior<TRequest, TResponse> 
        : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CommandTransactionBehavior(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            await _unitOfWork.StartTransaction();
            var response = await next();
            await _unitOfWork.CommitTransaction();

            return response;
        }
    }
}
