using System;

namespace CalendarApp.Services;

public class NotificationMessage
{
    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string Color { get; set; } = "#4285f4";
}

public interface INotificationService
{
    event Action<NotificationMessage>? OnNotificationReceived;
    void ShowNotification(string title, string message, string color = "#4285f4");
}

public class NotificationService : INotificationService
{
    public event Action<NotificationMessage>? OnNotificationReceived;

    public void ShowNotification(string title, string message, string color = "#4285f4")
    {
        OnNotificationReceived?.Invoke(new NotificationMessage 
        { 
            Title = title, 
            Message = message, 
            Color = color 
        });
    }
}
