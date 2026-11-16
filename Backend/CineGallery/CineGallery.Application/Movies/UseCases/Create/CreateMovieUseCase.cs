using CineGallery.Application.Common;
using CineGallery.Application.Movies.Events;
using CineGallery.Application.Movies.Models;
using CineGallery.Application.Movies.Repositories;
using CineGallery.Domain.Movies.Entities;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace CineGallery.Application.Movies.UseCases.Create
{
    public sealed class CreateMovieUseCase(
        IMovieRepository repository, 
        IValidator<MovieInput> validator,
        IPublisher publisher,
        ILogger<CreateMovieUseCase> logger)
    {
        private Activity? MovieActivity { get; set; }

        public async Task<Result<MovieOutput>> ExecuteAsync(MovieInput input, CancellationToken cancellationToken)
        {
            using (MovieActivity = MovieTelemetry.ActivitySource.StartActivity("movies.create"))
            {
                if (input is null) return Result<MovieOutput>.Failure(["Input cannot be null"]);

                var validationResult = await validator.ValidateAsync(input, options => options.IncludeRuleSets("Create", "default"), cancellationToken);

                if (!validationResult.IsValid) return Result<MovieOutput>.Failure([.. validationResult.Errors.Select(x => x.ErrorMessage)]);

                var movie = Movie.Create(input.Name);

                await repository.AddAsync(movie, cancellationToken);

                var result = await repository.SaveChangesAsync(cancellationToken);

                if (!result.IsSuccess) return Result<MovieOutput>.Failure([$"Error creating the movie {input.Name}"]);

                await PublishMovieChangedEventAsync(movie, cancellationToken);

                RegisterActivity(movie.Id);

                return Result<MovieOutput>.Success(new MovieOutput
                {
                    Id = movie.Id,
                    Name = movie.Name,
                    RowVersion = Convert.ToBase64String(movie.RowVersion)
                });
            }
        }

        private void RegisterActivity(Guid id)
        {
            MovieActivity?.SetTag("movie.id", id.ToString());
            MovieTelemetry.MoviesCreated.Add(1);
            logger.LogInformation("Movie created successfully. Id: {MovieId}", id);
        }

        private async Task PublishMovieChangedEventAsync(Movie movie, CancellationToken cancellationToken) 
            => await publisher.Publish(new MovieChangedEvent(movie.Id), cancellationToken);        
    }
}
