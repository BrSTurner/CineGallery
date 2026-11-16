using CineGallery.Application.Movies.Models;
using CineGallery.Application.Movies.Repositories;
using CineGallery.Domain.Movies.Constants;
using FluentValidation;

namespace CineGallery.Application.Movies.Validators
{
    public class MovieValidator : AbstractValidator<MovieInput>
    {
        private readonly IMovieRepository _movieRepository;
        public MovieValidator(IMovieRepository movieRepository)
        {
            _movieRepository = movieRepository;

            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Movie name is required")
                .MaximumLength(MovieConstants.NAME_MAX_LENGTH)
                .WithMessage($"Movie name cannot be more than {MovieConstants.NAME_MAX_LENGTH} characters");

            RuleSet("Create", ValidateCreate);
            RuleSet("Update", ValidateUpdate);
        }

        private void ValidateCreate()
        {
            RuleFor(x => x.Id)
                .Empty()
                .WithMessage("A new Movie cannot have an Id");

            RuleFor(x => x).CustomAsync(async (model, context, cancellationToken) =>
            {
                if (await _movieRepository.ExistsByNameAsync(model.Name, cancellationToken))
                {
                    context.AddFailure(nameof(model.Name), $"Movie {model.Name} already exists");
                }
            });
        }

        private void ValidateUpdate()
        {
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

                var movie = await _movieRepository.GetByIdAsNoTrackingAsync(model.Id!.Value, cancellationToken);

                if (movie is null)
                {
                    context.AddFailure(nameof(model.Id), "Movie could not be found");
                    return;
                }

                if (!movie.RowVersion.SequenceEqual(expectedRowVersion))
                {
                    context.AddFailure(nameof(model.RowVersion), "Movie has been modified by another request");
                    return;
                }

                if (model.Name.Equals(movie.Name, StringComparison.OrdinalIgnoreCase))
                {
                    context.AddFailure(nameof(model.Name), $"Movie {model.Name} already exists");
                }
            });
        }
    }
}
