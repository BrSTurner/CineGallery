using CineGallery.Application.Movies.Caching;
using CineGallery.Application.Movies.Models;
using CineGallery.Application.Movies.Repositories;

namespace CineGallery.Application.Movies.UseCases.GetAll
{
    public sealed class GetAllMoviesUseCase(IMovieRepository repository, IMovieListCache cache)
    {
        public async Task<List<MovieOutput>> ExecuteAsync(CancellationToken cancellationToken)
        {
            var cached = await cache.GetAsync(cancellationToken);

            if (cached is not null) return cached;

            var movies = await repository.GetAllAsync(cancellationToken);
            var output = movies.Select(x => new MovieOutput
            {
                Id = x.Id,
                Name = x.Name,
                RowVersion = Convert.ToBase64String(x.RowVersion)
            }).ToList();

            await cache.SetAsync(output, cancellationToken);

            return output;
        }
    }
}
