namespace CineGallery.Application.Movies.Models
{
    public record MovieOutput
    {
        public Guid Id { get; init; }
        public string Name { get; init; } = string.Empty;
        public string RowVersion { get; init; } = string.Empty;
    }
}
