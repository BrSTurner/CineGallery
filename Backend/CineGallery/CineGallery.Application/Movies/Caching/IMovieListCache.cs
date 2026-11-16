using CineGallery.Application.Movies.Models;

namespace CineGallery.Application.Movies.Caching
{
    public interface IMovieListCache
    {
        Task<List<MovieOutput>?> GetAsync(CancellationToken cancellationToken);

        Task SetAsync(List<MovieOutput> movies, CancellationToken cancellationToken);

        Task InvalidateAsync(CancellationToken cancellationToken);
    }
}
