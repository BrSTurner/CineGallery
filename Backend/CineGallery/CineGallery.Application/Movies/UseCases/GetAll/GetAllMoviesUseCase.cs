using CineGallery.Application.Movies.Models;
using CineGallery.Application.Movies.Repositories;

namespace CineGallery.Application.Movies.UseCases.GetAll
{
    public sealed class GetAllMoviesUseCase(IMovieRepository repository)
    {
        public async Task<List<MovieOutput>> ExecuteAsync(CancellationToken cancellationToken)
        {
            //TODO: Implement cache

            var movies = await repository.GetAllAsync(cancellationToken);

            return [.. movies.Select(x => new MovieOutput
            {
                Id = x.Id,
                Name = x.Name,
                RowVersion = Convert.ToBase64String(x.RowVersion)
            })];
        }
    }
}
