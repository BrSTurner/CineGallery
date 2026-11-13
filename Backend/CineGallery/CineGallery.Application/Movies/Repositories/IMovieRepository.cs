using CineGallery.Application.Common;
using CineGallery.Domain.Movies.Entities;

namespace CineGallery.Application.Movies.Repositories
{
    public interface IMovieRepository
    {
        Task AddAsync(Movie movie, CancellationToken ct);
        Task<List<Movie>> GetAllAsync(CancellationToken ct);
        Task<Movie?> GetByIdAsync(Guid id, CancellationToken ct);
        Task<List<Movie>> GetByNameAsync(string name, CancellationToken ct);
        void Delete(Movie movie);
        Task<Result<bool>> SaveChangesAsync(CancellationToken ct);
    }
}
