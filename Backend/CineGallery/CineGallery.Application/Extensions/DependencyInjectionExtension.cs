using CineGallery.Application.Movies.Models;
using CineGallery.Application.Movies.UseCases.Create;
using CineGallery.Application.Movies.UseCases.Delete;
using CineGallery.Application.Movies.UseCases.GetAll;
using CineGallery.Application.Movies.UseCases.GetById;
using CineGallery.Application.Movies.UseCases.Update;
using CineGallery.Application.Movies.Validators;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace CineGallery.Application.Extensions
{
    public static class DependencyInjectionExtension
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<CreateMovieUseCase>();
            services.AddScoped<UpdateMovieUseCase>();
            services.AddScoped<DeleteMovieUseCase>();
            services.AddScoped<GetAllMoviesUseCase>();
            services.AddScoped<GetMovieByIdUseCase>();

            services.AddScoped<IValidator<MovieInput>, MovieValidator>();
            services.AddScoped<IValidator<DeleteMovieCommand>, DeleteMovieValidator>();

            return services;
        }
    }
}
