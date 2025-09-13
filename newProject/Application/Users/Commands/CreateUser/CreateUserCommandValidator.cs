using FluentValidation;
using newProject.Application.Users.Commands.CreateUser;

namespace newProject.Application.Users.Commands.CreateUser;

public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("Username is required")
            .Length(3, 30).WithMessage("Username must be between 3 and 30 characters")
            .Matches(@"^[a-zA-Z0-9_]+$").WithMessage("Username can only contain letters, numbers, and underscores");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Email must be a valid email address")
            .MaximumLength(255).WithMessage("Email cannot exceed 255 characters");

        RuleFor(x => x.DisplayName)
            .NotEmpty().WithMessage("Display name is required")
            .Length(1, 100).WithMessage("Display name must be between 1 and 100 characters");

        RuleFor(x => x.Bio)
            .MaximumLength(500).WithMessage("Bio cannot exceed 500 characters")
            .When(x => !string.IsNullOrEmpty(x.Bio));

        RuleFor(x => x.ProfilePictureUrl)
            .Must(BeAValidUrl).WithMessage("Profile picture URL must be a valid URL")
            .When(x => !string.IsNullOrEmpty(x.ProfilePictureUrl));
    }

    private static bool BeAValidUrl(string? url)
    {
        if (string.IsNullOrEmpty(url)) return true;
        return Uri.TryCreate(url, UriKind.Absolute, out _);
    }
} 