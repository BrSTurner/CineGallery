using CineGallery.Domain.Movies.Constants;

namespace CineGallery.Domain.Movies.Entities
{
    public class Movie
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public byte[] RowVersion { get; set; } = [];

        protected Movie(){}

        private Movie(Guid id, string name)
        {
            Id = id;
            Name = name;
        }

        public static Movie Create(string name)
        {
            ValidateName(name);
            return new Movie(Guid.CreateVersion7(), name);
        }

        public void Update(string name)
        {
            ValidateName(name);
            Name = name;
        }

        private static void ValidateName(string name)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(name);

            if (name.Length > MovieConstants.NAME_MAX_LENGTH)
                throw new ArgumentException($"Name must have up to {MovieConstants.NAME_MAX_LENGTH} characters", nameof(name));
        }
    }
}
