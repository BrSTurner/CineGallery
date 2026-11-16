namespace CineGallery.Application.Movies.UseCases.Delete
{
    public record DeleteMovieCommand
    {
        public Guid Id { get; set; }
        public string RowVersion { get; set; } = string.Empty;
    }
}
