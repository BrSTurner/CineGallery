using CineGallery.Application.Common;
using CineGallery.Application.Movies.Models;
using CineGallery.Application.Movies.Repositories;
using FluentValidation;
using System.Data;

namespace CineGallery.Application.Movies.UseCases.Update
{
    public sealed class UpdateMovieUseCase(IMovieRepository repository, IValidator<MovieInput> validator)
    {
        public async Task<Result<MovieOutput>> ExecuteAsync(MovieInput input, CancellationToken cancellationToken)
        {
            if (input is null) return Result<MovieOutput>.Failure(["Input cannot be null"]);

            var validationResult = await validator.ValidateAsync(input, options => options.IncludeRuleSets("Update", "default"), cancellationToken);

            if (!validationResult.IsValid) return Result<MovieOutput>.Failure([.. validationResult.Errors.Select(x => x.ErrorMessage)]);

            var movieToUpdate = await repository.GetByIdAndRowVersionAsync(input.Id!.Value, Convert.FromBase64String(input.RowVersion), cancellationToken);

            if(movieToUpdate == null) return Result<MovieOutput>.Failure(["Movie could not be found or has been modified"]);

            movieToUpdate.Update(input.Name);

            try
            {
                var result = await repository.SaveChangesAsync(cancellationToken);

                if (!result.IsSuccess) return Result<MovieOutput>.Failure([$"Error updating the movie {input.Name}"]);
            }
            catch(DBConcurrencyException)
            {
                return Result<MovieOutput>.Failure([$"Movie has been modified by another request"]);
            }
            catch
            {
                return Result<MovieOutput>.Failure(["Something went wrong updating the movie"]);
            }

            return Result<MovieOutput>.Success(new MovieOutput
            {
                Id = movieToUpdate.Id,
                Name = movieToUpdate.Name,
                RowVersion = Convert.ToBase64String(movieToUpdate.RowVersion)
            });
            
        }
    }
}
