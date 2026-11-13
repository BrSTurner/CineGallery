using CineGallery.Application.Common;
using CineGallery.Application.Movies.Repositories;

namespace CineGallery.Application.Movies.UseCases.Delete
{
    public class DeleteMovieUseCase(IMovieRepository repository)
    {
        public async Task<Result<Guid>> Execute(Guid id, CancellationToken ct)
        {
            if (id == Guid.Empty)
                return Result<Guid>.Failure(["Movie Id is mandatory"]);

            var movie = await repository.GetByIdAsync(id, ct);

            if(movie is null)
                return Result<Guid>.Failure(["Movie could not be found"]);

            repository.Delete(movie);

            var result = await repository.SaveChangesAsync(ct);
             
            if(!result.IsSuccess) 
                return Result<Guid>.Failure([$"Movie {movie.Name} could not be delted"]);

            return Result<Guid>.Success(movie.Id);
        }
    }
}
