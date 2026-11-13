using Microsoft.AspNetCore.Diagnostics;

namespace CineGallery.WebAPI.Exceptions
{
    public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            var (statusCode, title) = exception switch
            {
                ArgumentException => (400, "Invalid request"),
                KeyNotFoundException => (404, "Resource not found"),
                _ => (500, "Internal server error")
            };

            logger.LogError(exception,
                "Exception occurred: {Message}", exception.Message);

            await Results.Problem(
                statusCode: statusCode,
                title: title,
                detail: statusCode == 500
                    ? "An unexpected error occurred."
                    : exception.Message,
                extensions: new Dictionary<string, object?>
                {
                    ["traceId"] = httpContext.TraceIdentifier
                }
            ).ExecuteAsync(httpContext);

            return true;
        }
    }
}
