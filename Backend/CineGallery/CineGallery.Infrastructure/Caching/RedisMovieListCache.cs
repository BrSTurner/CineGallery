using CineGallery.Application.Movies.Caching;
using CineGallery.Application.Movies.Models;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace CineGallery.Infrastructure.Caching
{
    public class RedisMovieListCache(
        IDistributedCache cache,
        ILogger<RedisMovieListCache> logger) : IMovieListCache
    {
        private const string CacheKey = "movies:all:v1";

        public async Task<List<MovieOutput>?> GetAsync(CancellationToken cancellationToken)
        {
            try
            {
                var movies = await cache.GetStringAsync(CacheKey, cancellationToken);

                if (movies is null)
                {
                    logger.LogInformation("Redis cache MISS");
                    return null;
                }

                logger.LogInformation("Redis cache HIT");

                return JsonSerializer.Deserialize<List<MovieOutput>>(movies);
            }
            catch (Exception ex) when (!cancellationToken.IsCancellationRequested)
            {
                logger.LogWarning(ex, "Redis unavailable");
                return null;
            }
        }

        public async Task SetAsync(List<MovieOutput> movies, CancellationToken cancellationToken)
        {
            try
            {
                var options = new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(60)
                };

                await cache.SetStringAsync(CacheKey, JsonSerializer.Serialize(movies), options, cancellationToken);
            }
            catch (Exception ex) when (!cancellationToken.IsCancellationRequested)
            {
                logger.LogWarning(ex, "Redis cache write failed");
            }
        }

        public async Task InvalidateAsync(CancellationToken ct)
        {
            try
            {
                await cache.RemoveAsync(CacheKey, ct);
            }
            catch (Exception ex) when (!ct.IsCancellationRequested)
            {
                logger.LogWarning(ex, "Redis invalidation failed");
            }
        }
    }
}
