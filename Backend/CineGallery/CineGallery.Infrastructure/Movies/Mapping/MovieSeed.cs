using CineGallery.Domain.Movies.Entities;
using Microsoft.EntityFrameworkCore;

namespace CineGallery.Infrastructure.Movies.Mapping
{
    public static class MovieSeed
    {
        private static readonly string[] Movies =
        [
            "The Shawshank Redemption",
            "The Godfather",
            "The Dark Knight",
            "Pulp Fiction",
            "Forrest Gump",
            "Inception",
            "Interstellar",
            "Fight Club",
            "The Matrix",
            "The Lord of the Rings: The Return of the King"
        ];

        public static void Seed(DbContext context)
        {
            if (context.Set<Movie>().Any())
                return;
          
            context.Set<Movie>().AddRange(Movies
                .Select(Movie.Create)
                .ToList());

            context.SaveChanges();
        }
    }
}
