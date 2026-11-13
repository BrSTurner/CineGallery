using CineGallery.Application.Common;
using CineGallery.Application.Movies.Models;
using CineGallery.Application.Movies.Repositories;
using CineGallery.Domain.Movies.Entities;
using Microsoft.Extensions.Logging;

namespace CineGallery.Application.Movies.UseCases.Create
{
    public class CreateMovieUseCase(IMovieRepository repository, ILogger<CreateMovieUseCase> logger)
    {
        public async Task<Result<MovieDTO>> Execute(CreateMovieInput input, CancellationToken ct)
        {
            using var activity = MovieTelemetry.ActivitySource.StartActivity("movies.create");

            if (input is null)
                return Result<MovieDTO>.Failure(["Input cannot be null"]);

            var validationResult = await new CreateMovieValidator().ValidateAsync(input, ct);

            if (!validationResult.IsValid)
                return Result<MovieDTO>.Failure([.. validationResult.Errors.Select(x => x.ErrorMessage)]);

            var existentMovie = await repository.GetByNameAsync(input.Name, ct);

            if (existentMovie.Count > 0)
                return Result<MovieDTO>.Failure([$"Movie {input.Name} already exists"]);

            var movie = Movie.Create(input.Name);

            await repository.AddAsync(movie, ct);

            var result = await repository.SaveChangesAsync(ct);

            if (!result.IsSuccess)
                return Result<MovieDTO>.Failure([$"Error creating the movie {input.Name}"]);

            activity?.SetTag("movie.id", movie.Id.ToString());
            MovieTelemetry.MoviesCreated.Add(1);
            logger.LogInformation("Movie created successfully. Id: {MovieId}",movie.Id);

            return Result<MovieDTO>.Success(new MovieDTO
            {
                Id = movie.Id,
                Name = movie.Name,
            });
        }
    }
}
