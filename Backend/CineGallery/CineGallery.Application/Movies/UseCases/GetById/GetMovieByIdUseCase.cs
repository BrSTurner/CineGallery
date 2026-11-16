using CineGallery.Application.Common;
using CineGallery.Application.Movies.Models;
using CineGallery.Application.Movies.Repositories;

namespace CineGallery.Application.Movies.UseCases.GetById
{
    public sealed class GetMovieByIdUseCase(IMovieRepository repository)
    {
        public async Task<Result<MovieOutput>> ExecuteAsync(Guid id, CancellationToken ct)
        {
            var movie = await repository.GetByIdAsync(id, ct);

            if (movie is null)
                return Result<MovieOutput>.Failure(["Movie could not be found"]);

            return Result<MovieOutput>.Success(new MovieOutput
            {
                Id = movie.Id,
                Name = movie.Name,
                RowVersion =  Convert.ToBase64String(movie.RowVersion)
            });
        }
    }
}
