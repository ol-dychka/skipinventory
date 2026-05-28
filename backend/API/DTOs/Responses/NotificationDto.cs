namespace API.DTOs.Responses;

public class NotificationDto(NotificationType type, string message, bool isDisappearing)
{
    public string Type { get; set; } = type.ToString();
    public string Message { get; set; } = message;
    public bool IsDisappearing { get; set; } = isDisappearing;
}

public enum NotificationType
{
    info,
    success,
    error,
}
