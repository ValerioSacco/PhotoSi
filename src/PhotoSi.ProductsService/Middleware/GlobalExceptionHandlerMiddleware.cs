using PhotoSi.ProductsService.Features.Shared;

namespace PhotoSi.ProductsService.Middleware
{
    internal sealed class GlobalExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;

        public GlobalExceptionHandlerMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validation failed");
                context.Response.StatusCode = StatusCodes.Status422UnprocessableEntity;
                var errorResponse = new
                {
                    errors = ex.Errors.ToList()
                };
                await context.Response.WriteAsJsonAsync(errorResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception occurred. Message: {Message}", ex.Message);
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;

                var errorResponse = new
                {
                    message = ex.Message
                };

                await context.Response.WriteAsJsonAsync(errorResponse);
            }
        }

    }
}
