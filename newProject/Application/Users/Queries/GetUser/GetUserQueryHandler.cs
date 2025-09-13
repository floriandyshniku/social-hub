using MediatR;
using newProject.Domain.Users;
using newProject.Domain.Users.ValueObjects;

namespace newProject.Application.Users.Queries.GetUser;

public class GetUserQueryHandler : IRequestHandler<GetUserQuery, User?>
{
    private readonly IUserRepository _userRepository;

    public GetUserQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<User?> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        var userId = UserId.Create(request.UserId);
        return await _userRepository.GetByIdAsync(userId);
    }
} 