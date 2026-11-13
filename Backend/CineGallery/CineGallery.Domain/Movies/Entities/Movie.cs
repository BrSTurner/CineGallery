using CineGallery.Domain.Movies.Constants;

namespace CineGallery.Domain.Movies.Entities
{
    public class Movie
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;

        protected Movie(){}

        private Movie(Guid id, string name)
        {
            Id = id;
            Name = name;
        }

        public static Movie Create(string name)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(name);

            if (name.Length > MovieConstants.NAME_MAX_LENGTH)
                throw new ArgumentException("Name must have up to 256 characters", nameof(name));

            return new Movie(Guid.CreateVersion7(), name);
        }
    }
}
