using Microsoft.AspNetCore.SignalR;

namespace newProject.Infrastructure.RealTime;

public class NotificationHub : Hub
{
    public async Task JoinUserGroup(string userId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, $"user_{userId}");
    }

    public async Task LeaveUserGroup(string userId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"user_{userId}");
    }

    public async Task JoinPostGroup(string postId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, $"post_{postId}");
    }

    public async Task LeavePostGroup(string postId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"post_{postId}");
    }

    public override async Task OnConnectedAsync()
    {
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await base.OnDisconnectedAsync(exception);
    }
} 