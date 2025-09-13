using FluentValidation;
using newProject.Application.Posts.Commands.CreatePost;

namespace newProject.Application.Posts.Commands.CreatePost;

public class CreatePostCommandValidator : AbstractValidator<CreatePostCommand>
{
    public CreatePostCommandValidator()
    {
        RuleFor(x => x.AuthorId)
            .NotEmpty().WithMessage("Author ID is required");

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