using CineGallery.Application.Common;
using CineGallery.Application.Movies.Repositories;
using CineGallery.Domain.Movies.Entities;
using CineGallery.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace CineGallery.Infrastructure.Movies.Repositories
{
    public class MovieRepository(CineGalleryContext context) : IMovieRepository
    {
        private readonly DbSet<Movie> _movies = context.Movies;

        public async Task AddAsync(Movie movie, CancellationToken cancellationToken) => await _movies.AddAsync(movie, cancellationToken);      
        public Task<List<Movie>> GetAllAsync(CancellationToken cancellationToken) => _movies.AsNoTracking().ToListAsync(cancellationToken);
        public async Task<Movie?> GetByIdAndRowVersionAsync(Guid id, byte[] rowVersion, CancellationToken cancellationToken) => await _movies.FirstOrDefaultAsync(x => x.Id == id && x.RowVersion.SequenceEqual(rowVersion), cancellationToken);
        public async Task<Movie?> GetByIdAsync(Guid id, CancellationToken cancellationToken) => await _movies.FindAsync(keyValues: [id], cancellationToken: cancellationToken);
        public async Task<Movie?> GetByIdAsNoTrackingAsync(Guid id, CancellationToken cancellationToken) => await _movies
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        
        public Task<List<Movie>> GetByNameAsync(string name, CancellationToken cancellationToken) => _movies
            .AsNoTracking()
            .Where(x => x.Name == name)
            .ToListAsync(cancellationToken);

        public async Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken) => await _movies.AnyAsync(x => x.Name == name, cancellationToken);
        
        public void Delete(Movie movie) 
        {
            SetRowVersionToCompare(movie, movie.RowVersion);
            _movies.Remove(movie); 
        }

        public void SetRowVersionToCompare(
            Movie movie,
            byte[] expectedRowVersion)
        {
            context.Entry(movie)
                .Property(x => x.RowVersion)
                .OriginalValue = expectedRowVersion;
        }

        public async Task<Result<bool>> SaveChangesAsync(CancellationToken cancellationToken)
        {
            try
            {
                await context.SaveChangesAsync(cancellationToken);
                return Result<bool>.Success(true);
            }
            catch(DbUpdateConcurrencyException)
            {
                throw new DBConcurrencyException("The model has been modified by another request");
            }
            catch (Exception)
            {
                return Result<bool>.Failure(["Error saving changes."]);
            }
        }
    }
}
