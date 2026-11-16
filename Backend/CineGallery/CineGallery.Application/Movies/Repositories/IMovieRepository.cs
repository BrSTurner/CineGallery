using CineGallery.Application.Common;
using CineGallery.Domain.Movies.Entities;

namespace CineGallery.Application.Movies.Repositories
{
    public interface IMovieRepository
    {
        Task AddAsync(Movie movie, CancellationToken cancellationToken);
        Task<List<Movie>> GetAllAsync(CancellationToken cancellationToken);
        Task<Movie?> GetByIdAndRowVersionAsync(Guid id, byte[] rowVersion, CancellationToken cancellationToken);
        Task<List<Movie>> GetByNameAsync(string name, CancellationToken cancellationToken);
        void Delete(Movie movie);
        void SetRowVersionToCompare(Movie movie, byte[] expectedRowVersion);
        Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken);
        Task<Result<bool>> SaveChangesAsync(CancellationToken cancellationToken);
        Task<Movie?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<Movie?> GetByIdAsNoTrackingAsync(Guid id, CancellationToken cancellationToken);
    }
}
