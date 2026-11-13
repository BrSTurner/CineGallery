using Asp.Versioning;
using CineGallery.Application.Extensions;
using CineGallery.Infrastructure.Extensions;
using CineGallery.WebAPI.Exceptions;
using CineGallery.WebAPI.Extensions;
using CineGallery.WebAPI.Versions;
using CineGallery.WebAPI.Versions.V1;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddApiVersioning(options =>
{
    options.ReportApiVersions = true;
    options.ApiVersionReader = new UrlSegmentApiVersionReader();
});

builder.Services.AddHealthChecks();
builder.Services.AddObservability();
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseExceptionHandler();
app.UseHttpsRedirection();

var versionedApi = app.NewVersionedApi("CineGallery");

versionedApi
    .BuildVersionedApi(1.0)
    .MapVersion1Endpoints();

app.MapHealthChecks("/health");

app.Run();