using CineGallery.Application.Common;
using CineGallery.Application.Movies.Repositories;
using CineGallery.Domain.Movies.Entities;
using CineGallery.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace CineGallery.Infrastructure.Movies.Repositories
{
    public class MovieRepository(CineGalleryContext context) : IMovieRepository
    {
        private readonly DbSet<Movie> _movies = context.Movies;

        public async Task AddAsync(Movie movie, CancellationToken ct) => await _movies.AddAsync(movie, ct);
        
        public async Task<List<Movie>> GetAllAsync(CancellationToken ct) => await _movies.AsNoTracking().ToListAsync(ct);

        public async Task<Movie?> GetByIdAsync(Guid id, CancellationToken ct) => await _movies.FindAsync(keyValues: [id], cancellationToken: ct);

        public async Task<List<Movie>> GetByNameAsync(string name, CancellationToken ct) => await _movies
            .AsNoTracking()
            .Where(x => x.Name.ToLower().Contains(name.ToLower()))
            .ToListAsync(ct);

        public void Delete(Movie movie) => _movies.Remove(movie);

        public async Task<Result<bool>> SaveChangesAsync(CancellationToken ct)
        {
            try
            {
                await context.SaveChangesAsync(ct);
                return Result<bool>.Success(true);
            }
            catch (Exception)
            {
                return Result<bool>.Failure(["Error saving changes."]);
            }
        }
    }
}
