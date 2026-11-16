using CineGallery.WebAPI.Auth;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;

namespace CineGallery.WebAPI.Extensions
{
    public static class AuthenticationExtensions
    {
        public static IServiceCollection AddCineAuthenticationAndAuthorization(this IServiceCollection services, IConfigurationManager configuration)
        {
            var spaOrigins = configuration.GetSection("Cors:Origins").Get<string[]>();

            if (spaOrigins is null || spaOrigins.Length == 0)
                spaOrigins = ["http://localhost:5173"];

            services.AddCors(options =>
            {
                options.AddPolicy("Spa", policy =>
                    policy.WithOrigins(spaOrigins)
                        .AllowAnyHeader()
                        .AllowAnyMethod());
            });

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddMicrosoftIdentityWebApi(configuration.GetSection("AzureAd"));

            var moviesWriteScope = configuration["AzureAd:Scope"];

            if (string.IsNullOrWhiteSpace(moviesWriteScope))
                moviesWriteScope = "Movies.Write";

            services.AddAuthorizationBuilder()
                    .AddPolicy(AuthorizationPolicies.MoviesWrite, policy => policy.RequireScope(moviesWriteScope));

            return services;
        }
    }
}
