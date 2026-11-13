using CineGallery.Application.Movies.Models;
using CineGallery.Application.Movies.UseCases.Create;
using CineGallery.Application.Movies.UseCases.Delete;
using CineGallery.Application.Movies.UseCases.GetAll;
using CineGallery.Application.Movies.UseCases.GetById;
using System.Net;

namespace CineGallery.WebAPI.Endpoints.V1
{
    public static class MoviesEndpoint
    {
        public static RouteGroupBuilder AddMoviesEndpoints(this RouteGroupBuilder group)
        {
            var moviesGroup = group
                                .MapGroup("/movies")
                                .WithTags("Movies");

            moviesGroup.MapGet("/{id:Guid}", async (Guid id, GetMovieByIdUseCase useCase, CancellationToken ct) =>
            {
                if (id == Guid.Empty)
                    return Results.BadRequest("Id cannot be null");

                var result = await useCase.Execute(id, ct);

                if (result.IsSuccess) return Results.Ok(result.Data);

                return Results.Conflict(result.Errors);
            })
            .WithTags("GetById")
            .WithName("Get by id")
            .WithDescription("Gets a Movie by its Id")
            .Produces<MovieDTO>()
            .ProducesProblem((int)HttpStatusCode.BadRequest)
            .ProducesProblem((int)HttpStatusCode.Conflict);

            moviesGroup.MapGet(string.Empty, async (GetAllMoviesUseCase usecase, CancellationToken ct) =>
            {
                return Results.Ok(await usecase.Execute(ct));
            })
            .WithTags("GetAll")
            .WithName("Get all")
            .WithDescription("Gets all Movies")
            .Produces<List<MovieDTO>>();

            moviesGroup.MapPost(string.Empty, async (CreateMovieInput input, CreateMovieUseCase useCase, CancellationToken ct) =>
            {
                if (input is null)
                    return Results.BadRequest("Input cannot be null");

                var result = await useCase.Execute(input, ct);

                if (result.IsSuccess) return Results.Created($"/{result.Data!.Id}", result.Data);

                return Results.Conflict(result.Errors);
            })
            .WithTags("Create")
            .WithName("Create Movie")
            .WithDescription("Creates a new Movie")
            .Produces<MovieDTO>((int)HttpStatusCode.Created)
            .ProducesProblem((int)HttpStatusCode.BadRequest)
            .ProducesProblem((int)HttpStatusCode.Conflict);

            moviesGroup.MapDelete("/{id:Guid}", async (Guid id, DeleteMovieUseCase useCase, CancellationToken ct) =>
            {
                if (id == Guid.Empty)
                    return Results.BadRequest("Id cannot be null");

                var result = await useCase.Execute(id, ct);

                if (result.IsSuccess) return Results.NoContent();

                return Results.Conflict(result.Errors);
            })
            .WithTags("Delete")
            .WithName("Delete Movie")
            .WithDescription("Deletes a Movie")
            .Produces((int)HttpStatusCode.NoContent)
            .ProducesProblem((int)HttpStatusCode.BadRequest)
            .ProducesProblem((int)HttpStatusCode.Conflict);

            return moviesGroup;
        }
    }
}
