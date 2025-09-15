using FluentValidation;

namespace SocialHub.Application.Posts.Commands.EditPost
{
    public class EditPostCommandValidator : AbstractValidator<EditPostCommand>
    {
        public EditPostCommandValidator()
        {

            RuleFor(x => x.Content)
                .NotEmpty().WithMessage("Post content is required")
                .Length(1, 280).WithMessage("Post content must be between 1 and 280 characters");

            RuleFor(x => x.ImageUrl)
                .Must(BeAValidUrl).WithMessage("Image URL must be a valid URL")
                .When(x => !string.IsNullOrEmpty(x.ImageUrl));
        }

        private static bool BeAValidUrl(string? url)
        {
            if (string.IsNullOrEmpty(url)) return true;
            return Uri.TryCreate(url, UriKind.Absolute, out _);
        }
    }
}
