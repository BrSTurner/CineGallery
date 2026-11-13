namespace CineGallery.Application.Movies.Models
{
    public record MovieDTO
    {
        public Guid Id { get; init; }
        public string Name { get; init; } = string.Empty;
    }
}
