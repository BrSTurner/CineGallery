using CineGallery.Application.Movies.Models;
using CineGallery.Application.Movies.UseCases.Create;
using CineGallery.Application.Movies.UseCases.Delete;
using CineGallery.Application.Movies.UseCases.GetAll;
using CineGallery.Application.Movies.UseCases.GetById;
using CineGallery.Application.Movies.UseCases.Update;
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

            moviesGroup.MapGet("/{id:Guid:required}", async (Guid id, GetMovieByIdUseCase useCase, CancellationToken ct) =>
            {
                if (id == Guid.Empty)
                    return Results.BadRequest("Id cannot be null");

                var result = await useCase.ExecuteAsync(id, ct);

                if (result.IsSuccess) return Results.Ok(result.Data);

                return Results.NotFound(result.Errors);
            })
            .WithTags("GetById")
            .WithName("Get by id")
            .WithDescription("Gets a Movie by its Id")
            .Produces<MovieOutput>()
            .ProducesProblem((int)HttpStatusCode.BadRequest)
            .ProducesProblem((int)HttpStatusCode.NotFound);

            moviesGroup.MapGet(string.Empty, async (GetAllMoviesUseCase usecase, CancellationToken ct) =>
            {
                return Results.Ok(await usecase.ExecuteAsync(ct));
            })
            .WithTags("GetAll")
            .WithName("Get all")
            .WithDescription("Gets all Movies")
            .Produces<List<MovieOutput>>();

            moviesGroup.MapPost(string.Empty, async (MovieInput input, CreateMovieUseCase useCase, CancellationToken ct) =>
            {
                if (input is null)
                    return Results.BadRequest("Input cannot be null");

                var result = await useCase.ExecuteAsync(input, ct);

                if (result.IsSuccess) return Results.Created($"/{result.Data!.Id}", result.Data);

                return Results.Conflict(result.Errors);
            })
            .WithTags("Create")
            .WithName("Create Movie")
            .WithDescription("Creates a new Movie")
            .Produces<MovieOutput>((int)HttpStatusCode.Created)
            .ProducesProblem((int)HttpStatusCode.BadRequest)
            .ProducesProblem((int)HttpStatusCode.Conflict);

            moviesGroup.MapPut(string.Empty, async (MovieInput input, UpdateMovieUseCase useCase, CancellationToken ct) =>
            {
                if (input is null)
                    return Results.BadRequest("Input cannot be null");

                var result = await useCase.ExecuteAsync(input, ct);

                if (result.IsSuccess) return Results.Accepted($"/{result.Data!.Id}", result.Data);

                return Results.Conflict(result.Errors);
            })
            .WithTags("Update")
            .WithName("Update Movie")
            .WithDescription("Updates an existent Movie")
            .Produces<MovieOutput>((int)HttpStatusCode.Accepted)
            .ProducesProblem((int)HttpStatusCode.BadRequest)
            .ProducesProblem((int)HttpStatusCode.Conflict);

            moviesGroup.MapDelete("/{id:Guid:required}/{rowVersion:required}", async (Guid id, string rowVersion, DeleteMovieUseCase useCase, CancellationToken ct) =>
            {
                if (id == Guid.Empty)
                    return Results.BadRequest("Id cannot be null");

                if (string.IsNullOrEmpty(rowVersion))
                    return Results.BadRequest("Row Version cannot be empty");

                var result = await useCase.ExecuteAsync(new DeleteMovieCommand { Id = id, RowVersion = rowVersion }, ct);

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
