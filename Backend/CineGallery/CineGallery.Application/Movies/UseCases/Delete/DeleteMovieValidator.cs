using CineGallery.Application.Movies.Repositories;
using FluentValidation;

namespace CineGallery.Application.Movies.UseCases.Delete
{
    public class DeleteMovieValidator : AbstractValidator<DeleteMovieCommand>
    {
        private readonly IMovieRepository _movieRepository;

        public DeleteMovieValidator(IMovieRepository movieRepository)
        {
            _movieRepository = movieRepository;

            RuleFor(x => x.Id)
               .NotEmpty()
               .WithMessage("Movie Id is required");

            RuleFor(x => x.RowVersion)
                .NotEmpty()
                .Must(value =>
                {
                    return Convert.TryFromBase64String(
                        value,
                        new Span<byte>(new byte[8]),
                        out var bytesWritten
                    ) && bytesWritten == 8;
                })
                .WithMessage("Invalid RowVersion.");


            RuleFor(x => x).CustomAsync(async (model, context, cancellationToken) =>
            {
                var expectedRowVersion = Convert.FromBase64String(model.RowVersion);

                var movie = await _movieRepository.GetByIdAsNoTrackingAsync(model.Id, cancellationToken);

                if (movie is null)
                {
                    context.AddFailure(nameof(model.Id), "Movie could not be found");
                    return;
                }

                if (!movie.RowVersion.SequenceEqual(expectedRowVersion))
                {
                    context.AddFailure(nameof(model.RowVersion), "Movie has been deleted or modified by another request");
                    return;
                }
            });
        }
    }
}
