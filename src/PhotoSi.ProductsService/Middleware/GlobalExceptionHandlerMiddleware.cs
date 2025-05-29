using PhotoSi.ProductsService.Exceptions;
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
                    message = "Input validation error",
                    errors = ex.Errors.ToList()
                };
                await context.Response.WriteAsJsonAsync(errorResponse);
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex, "Entity not found");
                context.Response.StatusCode = StatusCodes.Status404NotFound;
                var errorResponse = new
                {
                    message = "Item not found",
                    errors = ex.Message
                };
                await context.Response.WriteAsJsonAsync(errorResponse);
            }
            catch (BusinessRuleException ex)
            {
                _logger.LogWarning(ex, "Business rule violation");
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                var errorResponse = new
                {
                    message = "Business rule violation",
                    errors = ex.Message
                };
                await context.Response.WriteAsJsonAsync(errorResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception occurred. Message: {Message}", ex.Message);
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;

                var errorResponse = new
                {
                    message = "Internal server error",
                    error = ex.Message
                };

                await context.Response.WriteAsJsonAsync(errorResponse);
            }
        }

    }
}
