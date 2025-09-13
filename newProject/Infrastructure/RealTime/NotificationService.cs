using Microsoft.AspNetCore.SignalR;

namespace newProject.Infrastructure.RealTime;

public class NotificationService : INotificationService
{
    private readonly IHubContext<NotificationHub> _hubContext;

    public NotificationService(IHubContext<NotificationHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public async Task NotifyUserAsync(string userId, string message, string type, object? data = null)
    {
        var notification = new
        {
            Message = message,
            Type = type,
            Data = data,
            Timestamp = DateTime.UtcNow
        };

        await _hubContext.Clients.Group($"user_{userId}").SendAsync("ReceiveNotification", notification);
    }

    public async Task NotifyPostGroupAsync(string postId, string message, string type, object? data = null)
    {
        var notification = new
        {
            Message = message,
            Type = type,
            Data = data,
            Timestamp = DateTime.UtcNow
        };

        await _hubContext.Clients.Group($"post_{postId}").SendAsync("ReceivePostUpdate", notification);
    }

    public async Task NotifyAllAsync(string message, string type, object? data = null)
    {
        var notification = new
        {
            Message = message,
            Type = type,
            Data = data,
            Timestamp = DateTime.UtcNow
        };

        await _hubContext.Clients.All.SendAsync("ReceiveBroadcast", notification);
    }
} 