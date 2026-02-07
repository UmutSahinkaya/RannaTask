using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RannaTask.Business.Notifications;
using System.Security.Claims;

namespace RannaTask.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        /// <summary>
        /// Get my notifications
        /// </summary>
        [HttpGet("my-notifications")]
        public async Task<IActionResult> GetMyNotifications()
        {
            var userId = GetUserIdFromToken();
            if (userId == 0)
                return Unauthorized(new { message = "Kullanıcı kimliği bulunamadı" });

            var notifications = await _notificationService.GetUserNotificationsAsync(userId);
            return Ok(notifications);
        }

        /// <summary>
        /// Get unread count
        /// </summary>
        [HttpGet("unread-count")]
        public async Task<IActionResult> GetUnreadCount()
        {
            var userId = GetUserIdFromToken();
            if (userId == 0)
                return Unauthorized(new { message = "Kullanıcı kimliği bulunamadı" });

            var count = await _notificationService.GetUnreadCountAsync(userId);
            return Ok(new { count });
        }

        /// <summary>
        /// Mark notification as read
        /// </summary>
        [HttpPut("{id}/mark-read")]
        public async Task<IActionResult> MarkAsRead(int id)
        {
            var result = await _notificationService.MarkAsReadAsync(id);
            if (!string.IsNullOrEmpty(result.Message))
                return BadRequest(new { message = result.Message });

            return Ok(new { message = "Bildirim okundu" });
        }

        /// <summary>
        /// Mark all as read
        /// </summary>
        [HttpPut("mark-all-read")]
        public async Task<IActionResult> MarkAllAsRead()
        {
            var userId = GetUserIdFromToken();
            if (userId == 0)
                return Unauthorized(new { message = "Kullanıcı kimliği bulunamadı" });

            var result = await _notificationService.MarkAllAsReadAsync(userId);
            return Ok(new { message = "Tüm bildirimler okundu" });
        }

        private int GetUserIdFromToken()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return !string.IsNullOrEmpty(userIdClaim) && int.TryParse(userIdClaim, out int userId) ? userId : 0;
        }
    }
}
