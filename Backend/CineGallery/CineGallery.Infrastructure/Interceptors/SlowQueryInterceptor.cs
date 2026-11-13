using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;
using System.Data.Common;

namespace CineGallery.Infrastructure.Interceptors
{
    public class SlowQueryInterceptor(ILogger<SlowQueryInterceptor> logger) : DbCommandInterceptor
    {
        public override ValueTask<DbDataReader> ReaderExecutedAsync(
            DbCommand command,
            CommandExecutedEventData eventData,
            DbDataReader result,
            CancellationToken cancellationToken = default)
        {
            if (eventData.Duration.TotalMilliseconds > 0.100)
                logger.LogWarning("Slow query detected: {DurationMs}ms", eventData.Duration.TotalMilliseconds);
            
            return new ValueTask<DbDataReader>(result);
        }
    }
}
