using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace CineGallery.WebAPI.Extensions
{
    public static class ObservabilityExtensions
    {
        public static IServiceCollection AddObservability(this IServiceCollection services)
        {
            services.AddOpenTelemetry()
                .ConfigureResource(resource =>
                    resource.AddService("CineGallery.WebAPI"))

                .WithTracing(tracing => tracing
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddSource("CineGallery.Application")
                    .AddOtlpExporter())

                .WithMetrics(metrics => metrics
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddRuntimeInstrumentation()
                    .AddMeter("CineGallery.Application")
                    .AddOtlpExporter())

                .WithLogging(logging =>
                {
                    logging.AddOtlpExporter();
                });

            return services;
        }
    }
}
