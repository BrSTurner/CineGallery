using CineGallery.Domain.Movies.Entities;
using CineGallery.Infrastructure.Movies.Mapping;
using Microsoft.EntityFrameworkCore;

namespace CineGallery.Infrastructure.Context
{
    public class CineGalleryContext(DbContextOptions options) : DbContext(options)
    {
        public DbSet<Movie> Movies { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(MovieMapping).Assembly);
        }
    }
}
