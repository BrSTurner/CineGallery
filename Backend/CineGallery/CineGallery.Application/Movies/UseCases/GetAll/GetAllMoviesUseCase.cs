using CineGallery.Application.Movies.Models;
using CineGallery.Application.Movies.Repositories;

namespace CineGallery.Application.Movies.UseCases.GetAll
{
    public class GetAllMoviesUseCase(IMovieRepository repository)
    {
        public async Task<List<MovieDTO>> Execute(CancellationToken ct)
        {
            var movies = await repository.GetAllAsync(ct);

            return [.. movies.Select(x => new MovieDTO
            {
                Id = x.Id,
                Name = x.Name
            })];
        }
    }
}
