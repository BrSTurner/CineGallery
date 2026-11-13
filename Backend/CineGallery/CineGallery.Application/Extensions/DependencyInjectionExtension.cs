using CineGallery.Application.Movies.UseCases.Create;
using CineGallery.Application.Movies.UseCases.Delete;
using CineGallery.Application.Movies.UseCases.GetAll;
using CineGallery.Application.Movies.UseCases.GetById;
using Microsoft.Extensions.DependencyInjection;

namespace CineGallery.Application.Extensions
{
    public static class DependencyInjectionExtension
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<CreateMovieUseCase>();
            services.AddScoped<DeleteMovieUseCase>();
            services.AddScoped<GetAllMoviesUseCase>();
            services.AddScoped<GetMovieByIdUseCase>();

            return services;
        }
    }
}
