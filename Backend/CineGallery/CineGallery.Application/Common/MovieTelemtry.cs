using System.Diagnostics;
using System.Diagnostics.Metrics;

namespace CineGallery.Application.Common
{
    public static class MovieTelemetry
    {
        public const string Name = "CineGallery.Application";

        public static readonly ActivitySource ActivitySource =
            new(Name);

        public static readonly Meter Meter =
            new(Name);

        public static readonly Counter<long> MoviesCreated =
            Meter.CreateCounter<long>("cinegallery.movies.created");
    }
}
