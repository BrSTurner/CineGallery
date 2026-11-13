namespace CineGallery.Application.Movies.UseCases.Create
{
    public record CreateMovieInput 
    {
        public string Name { get; init; } = string.Empty;
    }
}
