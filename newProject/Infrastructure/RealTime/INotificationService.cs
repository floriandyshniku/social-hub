namespace newProject.Infrastructure.RealTime;

public interface INotificationService
{
    Task NotifyUserAsync(string userId, string message, string type, object? data = null);
    Task NotifyPostGroupAsync(string postId, string message, string type, object? data = null);
    Task NotifyAllAsync(string message, string type, object? data = null);
} 