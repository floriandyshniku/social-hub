using MediatR;
using newProject.Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace newProject.Application.Users.Queries.GetAllUsers;

public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, IEnumerable<User>>
{
    private readonly IUserRepository _userRepository;

    public GetAllUsersQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<IEnumerable<User>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        // Validate query parameters
        if (request.MaxResults <= 0 || request.MaxResults > 1000)
        {
            throw new ArgumentException("MaxResults must be between 1 and 1000");
        }    

        var userNameContains = request.UsernameContains;
        var createdAfter = request.CreatedAfter;
        var maxResults = request.MaxResults;

        var queryable = _userRepository.GetAll();

        // Apply filters at database level
        if (!string.IsNullOrEmpty(userNameContains))
        {
            queryable = queryable.Where(x => x.Username.Value.Contains(userNameContains));
        }

        if (createdAfter.HasValue)
        {
            queryable = queryable.Where(x => x.CreatedAt >= createdAfter.Value);
        }

        // Apply pagination and execute query
        var filteredUsers = await queryable
            .Take(maxResults)
            .ToListAsync(cancellationToken);

        return filteredUsers;
    }
} 