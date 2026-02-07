using RannaTask.Entities.Common;
using System;

namespace RannaTask.Entities.Entities
{
    public class Notification : BaseEntity<int>
    {
        public string Title { get; set; }
        public string Message { get; set; }
        public NotificationType Type { get; set; }
        public int? RelatedEntityId { get; set; }  // SupportFormId, ProductId, etc.
        public string RelatedEntityType { get; set; }  // "SupportForm", "Product", etc.
        public bool IsRead { get; set; } = false;
        public int? UserId { get; set; }  // null = all admins, or specific user
        
        public User User { get; set; }
    }
}
