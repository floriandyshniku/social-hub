using MediatR;
using newProject.Domain.Users;

namespace newProject.Application.Users.Queries.GetAllUsers;

public class GetAllUsersQuery : IRequest<IEnumerable<User>>
{
    public string? UsernameContains { get; set; }
    public DateTime? CreatedAfter { get; set; }
    public int MaxResults { get; set; } = 50;

    public GetAllUsersQuery()
    {
        // Ensure MaxResults is within reasonable bounds
        if (MaxResults <= 0 || MaxResults > 1000)
        {
            MaxResults = 50;
        }
    }
} 