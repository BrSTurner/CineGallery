using CineGallery.Application.Common;
using CineGallery.Application.Movies.Models;
using CineGallery.Application.Movies.Repositories;

namespace CineGallery.Application.Movies.UseCases.GetById
{
    public class GetMovieByIdUseCase(IMovieRepository repository)
    {
        public async Task<Result<MovieDTO>> Execute(Guid id, CancellationToken ct)
        {
            var movie = await repository.GetByIdAsync(id, ct);

            if (movie is null)
                return Result<MovieDTO>.Failure(["Movie could not be found"]);

            return Result<MovieDTO>.Success(new MovieDTO
            {
                Id = movie.Id,
                Name = movie.Name
            });
        }
    }
}
