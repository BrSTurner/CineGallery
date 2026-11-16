namespace CineGallery.Application.Movies.Models
{
    public record MovieInput
    {
        public Guid? Id { get; init; } = null;      
        public string Name { get; init; } = string.Empty;
        public string RowVersion { get; init; } = string.Empty;
    }
}
