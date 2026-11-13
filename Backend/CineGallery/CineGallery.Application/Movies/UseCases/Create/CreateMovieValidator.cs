using CineGallery.Domain.Movies.Constants;
using FluentValidation;

namespace CineGallery.Application.Movies.UseCases.Create
{
    public class CreateMovieValidator : AbstractValidator<CreateMovieInput>
    {
        public CreateMovieValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Movie name is required")
                .MaximumLength(MovieConstants.NAME_MAX_LENGTH)
                .WithMessage($"Movie name cannot be more than {MovieConstants.NAME_MAX_LENGTH} characters");
        }
    }
}
