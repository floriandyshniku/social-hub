using Microsoft.AspNetCore.Mvc;
using newProject.Application.Users.Commands.CreateUser;
using newProject.Application.Users.Commands.UpdateUser;
using newProject.Application.Users.Commands.DeleteUser;
using newProject.Application.Users.Queries.GetAllUsers;
using newProject.Application.Users.Queries.GetUser;
using newProject.Application.Users.Commands.FollowUser;
using MediatR;

namespace newProject.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;

    public UsersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserCommand command)
    {
        var userId = await _mediator.Send(command);
        return Ok(new { UserId = userId.Value });
    }

    [HttpGet("{userId:guid}")]
    public async Task<IActionResult> GetUser(Guid userId)
    {
        var query = new GetUserQuery { UserId = userId };
        var user = await _mediator.Send(query);

        if (user == null)
            return NotFound();

        return Ok(new
        {
            Id = user.Id.Value,
            Username = user.Username.Value,
            Email = user.Email.Value,
            DisplayName = user.DisplayName,
            Bio = user.Bio,
            CreatedAt = user.CreatedAt,
            IsActive = user.IsActive
        });
    }

    [HttpGet]
    public async Task<IActionResult> GetAllUsers([FromQuery] GetAllUsersQuery query)
    {
        var users = await _mediator.Send(query);

        var response = users.Select(user => new
        {
            Id = user.Id.Value,
            Username = user.Username.Value,
            Email = user.Email.Value,
            DisplayName = user.DisplayName,
            Bio = user.Bio,
            CreatedAt = user.CreatedAt,
            IsActive = user.IsActive
        });

        return Ok(response);
    }

    [HttpPut("{userId:guid}")]
    public async Task<IActionResult> UpdateUser(Guid userId, [FromBody] UpdateUserCommand command)
    {
        command.UserId = userId;
        await _mediator.Send(command);
        return Ok(new { Message = "User updated successfully" });
    }

    [HttpDelete("{userId:guid}")]
    public async Task<IActionResult> DeleteUser(Guid userId, [FromQuery] bool hardDelete = false)
    {
        var command = new DeleteUserCommand 
        { 
            UserId = userId, 
            HardDelete = hardDelete 
        };
        
        await _mediator.Send(command);
        
        var message = hardDelete ? "User deleted permanently" : "User deactivated";
        return Ok(new { Message = message });
    }

    [HttpPost("{followerId:guid}/follow/{userToFollowId:guid}")]
    public async Task<IActionResult> FollowUser(Guid followerId, Guid userToFollowId)
    {
        var command = new FollowUserCommand
        {
            FollowerId = followerId,
            UserToFollowId = userToFollowId
        };

        await _mediator.Send(command);
        return Ok(new { Message = "User followed successfully" });
    }
} 