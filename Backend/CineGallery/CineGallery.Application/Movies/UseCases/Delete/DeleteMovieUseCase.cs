using CineGallery.Application.Common;
using CineGallery.Application.Movies.Repositories;
using FluentValidation;
using System.Data;

namespace CineGallery.Application.Movies.UseCases.Delete
{
    public sealed class DeleteMovieUseCase(IMovieRepository repository, IValidator<DeleteMovieCommand> validator)
    {
        public async Task<Result<Guid>> ExecuteAsync(DeleteMovieCommand command, CancellationToken cancellationToken)
        {
            if(command is null) return Result<Guid>.Failure(["Delete input is mandatory"]);

            var validationResult = await validator.ValidateAsync(command, cancellationToken);

            if (!validationResult.IsValid) return Result<Guid>.Failure([..validationResult.Errors.Select(x => x.ErrorMessage)]);

            var movie = await repository.GetByIdAndRowVersionAsync(command.Id, Convert.FromBase64String(command.RowVersion), cancellationToken);

            if(movie is null) return Result<Guid>.Failure(["Movie could not be found or has been modified"]);

            repository.Delete(movie);

            try
            {
                var result = await repository.SaveChangesAsync(cancellationToken);

                if (!result.IsSuccess) return Result<Guid>.Failure([$"Error deleting the movie {movie.Name}"]);
            }
            catch (DBConcurrencyException)
            {
                return Result<Guid>.Failure([$"Movie has been modified or deleted by another request"]);
            }
            catch
            {
                return Result<Guid>.Failure(["Something went wrong deleting the movie"]);
            }         

            return Result<Guid>.Success(movie.Id);
        }
    }
}
