using CineGallery.Application.Movies.Caching;
using CineGallery.Application.Movies.Repositories;
using CineGallery.Infrastructure.Caching;
using CineGallery.Infrastructure.Context;
using CineGallery.Infrastructure.Interceptors;
using CineGallery.Infrastructure.Movies.Mapping;
using CineGallery.Infrastructure.Movies.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CineGallery.Infrastructure.Extensions
{
    public static class DependencyInjectionExtension
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfigurationManager configuration)
        {

            services.AddDbContext<CineGalleryContext>((sp, options) =>
            {
                options
                    .UseSqlServer(configuration.GetConnectionString("DefaultConnection"))
                    .UseSeeding((context, _) =>
                    {
                        MovieSeed.Seed(context);
                    })
                    .AddInterceptors(sp.GetRequiredService<SlowQueryInterceptor>());
            });

            var redisConnection = configuration.GetConnectionString("Redis");

            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = redisConnection;
                options.InstanceName = "CineGallery:";
            });

            services.AddScoped<IMovieListCache, RedisMovieListCache>();
            services.AddScoped<SlowQueryInterceptor>();
            services.AddScoped<IMovieRepository, MovieRepository>();

            return services;
        }
    }
}
