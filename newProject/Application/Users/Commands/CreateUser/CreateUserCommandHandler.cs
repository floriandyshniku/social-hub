using MediatR;
using newProject.Domain.Users;
using newProject.Domain.Users.ValueObjects;

namespace newProject.Application.Users.Commands.CreateUser;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, UserId>
{
    private readonly IUserRepository _userRepository;

    public CreateUserCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<UserId> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        // Validate input
        if (string.IsNullOrWhiteSpace(request.Username))
            throw new ArgumentException("Username is required", nameof(request.Username));

        if (string.IsNullOrWhiteSpace(request.Email))
            throw new ArgumentException("Email is required", nameof(request.Email));

        if (string.IsNullOrWhiteSpace(request.DisplayName))
            throw new ArgumentException("Display name is required", nameof(request.DisplayName));

        // Create value objects
        var username = Username.Create(request.Username);
        var email = Email.Create(request.Email);

        // Check if user already exists
        var existingUserByUsername = await _userRepository.GetByUsernameAsync(username);
        if (existingUserByUsername != null)
            throw new InvalidOperationException($"Username '{request.Username}' is already taken");

        var existingUserByEmail = await _userRepository.GetByEmailAsync(email);
        if (existingUserByEmail != null)
            throw new InvalidOperationException($"Email '{request.Email}' is already registered");

        // Create user
        var user = User.Create(username, email, request.DisplayName);

        // Save to repository
        await _userRepository.AddAsync(user);

        return user.Id;
    }
} 