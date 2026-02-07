using RannaTask.Entities.Entities;

namespace RannaTask.Business.Notifications;

public interface INotificationService
{
    Task<NotificationDto> CreateAsync(CreateNotificationDto request);
    Task<List<NotificationDto>> GetUserNotificationsAsync(int userId);
    Task<List<NotificationDto>> GetAdminNotificationsAsync();
    Task<int> GetUnreadCountAsync(int userId);
    Task<NoContent> MarkAsReadAsync(int notificationId);
    Task<NoContent> MarkAllAsReadAsync(int userId);
}
