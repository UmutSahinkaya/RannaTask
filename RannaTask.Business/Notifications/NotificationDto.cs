using RannaTask.Entities.Common;

namespace RannaTask.Business.Notifications;

public class NotificationDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Message { get; set; }
    public NotificationType Type { get; set; }
    public int? RelatedEntityId { get; set; }
    public string RelatedEntityType { get; set; }
    public bool IsRead { get; set; }
    public int? UserId { get; set; }
    public DateTime Created { get; set; }
}

public class CreateNotificationDto
{
    public string Title { get; set; }
    public string Message { get; set; }
    public NotificationType Type { get; set; }
    public int? RelatedEntityId { get; set; }
    public string RelatedEntityType { get; set; }
    public int? UserId { get; set; }  // null = broadcast to all admins
}
