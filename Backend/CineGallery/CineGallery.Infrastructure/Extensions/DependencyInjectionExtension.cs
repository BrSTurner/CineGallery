using CineGallery.Application.Movies.Repositories;
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

            services.AddScoped<SlowQueryInterceptor>();
            services.AddScoped<IMovieRepository, MovieRepository>();

            return services;
        }
    }
}
