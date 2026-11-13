using Asp.Versioning.Builder;

namespace CineGallery.WebAPI.Versions
{
    public static class VersionConfigurator
    {
        public static RouteGroupBuilder BuildVersionedApi(this IVersionedEndpointRouteBuilder builder, double version, string? status = null)
        {
            return builder
                    .MapGroup("/api/v{version:apiVersion}")
                    .HasApiVersion(version, status);
        }
    }
}
