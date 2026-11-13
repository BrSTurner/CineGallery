using CineGallery.WebAPI.Endpoints.V1;

namespace CineGallery.WebAPI.Versions.V1
{
    public static class Version1Configurator
    {
        public static RouteGroupBuilder MapVersion1Endpoints(this RouteGroupBuilder builder)
        {
            return builder
                    .AddMoviesEndpoints();
        }
    }
}
