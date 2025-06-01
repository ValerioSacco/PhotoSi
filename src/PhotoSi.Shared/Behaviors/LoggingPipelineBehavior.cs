using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace PhotoSi.Shared.Behaviors
{
    public class LoggingPipelineBehavior<TRequest, TResponse> 
        : IPipelineBehavior<TRequest, TResponse> 
        where TRequest : IRequest<TResponse>
    {
        private readonly ILogger<LoggingPipelineBehavior<TRequest, TResponse>> _logger;

        public LoggingPipelineBehavior(ILogger<LoggingPipelineBehavior<TRequest, TResponse>> logger)
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            var requestName = typeof(TRequest).Name;
            _logger.LogInformation("Handling {RequestName} with payload: {@Request}", requestName, request);

            var stopwatch = Stopwatch.StartNew();
            try
            {
                var response = await next();
                stopwatch.Stop();

                _logger.LogInformation(
                    "Handled {RequestName} in {ElapsedMilliseconds}ms with response: {@Response}",
                    requestName, stopwatch.ElapsedMilliseconds, response);

                return response;
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                _logger.LogError(
                    ex,
                    "Exception handling {RequestName} after {ElapsedMilliseconds}ms. Payload: {@Request}",
                    requestName, stopwatch.ElapsedMilliseconds, request);
                throw;
            }
        }
    }
}
