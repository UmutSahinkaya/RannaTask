using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using RannaTask.API.Hubs;
using RannaTask.Business.Notifications;
using RannaTask.Business.SupportForms;
using RannaTask.Entities.Common;
using System.Security.Claims;

namespace RannaTask.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SupportFormController : ControllerBase
    {
        private readonly ISupportFormService _supportFormService;
        private readonly INotificationService _notificationService;
        private readonly IHubContext<NotificationHub> _hubContext;

        public SupportFormController(
            ISupportFormService supportFormService,
            INotificationService notificationService,
            IHubContext<NotificationHub> hubContext)
        {
            _supportFormService = supportFormService;
            _notificationService = notificationService;
            _hubContext = hubContext;
        }

        /// <summary>
        /// Get all support forms (Admin/Manager only)
        /// </summary>
        [HttpGet]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> GetAll()
        {
            var forms = await _supportFormService.GetAllListAsync();
            return Ok(forms);
        }

        /// <summary>
        /// Get my support forms (Customer)
        /// </summary>
        [HttpGet("my-forms")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> GetMyForms()
        {
            var userId = GetUserIdFromToken();
            if (userId == 0)
                return Unauthorized(new { message = "Kullanıcı kimliği bulunamadı" });

            var allForms = await _supportFormService.GetAllListAsync();
            var myForms = allForms.Where(f => f.UserId == userId).ToList();

            return Ok(myForms);
        }

        /// <summary>
        /// Create new support form (Customer)
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> Create([FromBody] CreateSupportFormRequest request)
        {
            try
            {
                var userId = GetUserIdFromToken();
                if (userId == 0)
                    return Unauthorized(new { message = "Kullanıcı kimliği bulunamadı" });

                var createDto = new CreateSupportFormDto(request.Subject, request.Message, userId);
                var result = await _supportFormService.CreateAsync(createDto);

                // Create notification for admins
                var notificationDto = new CreateNotificationDto
                {
                    Title = "Yeni Destek Talebi",
                    Message = $"'{request.Subject}' konulu yeni bir destek talebi oluşturuldu.",
                    Type = NotificationType.SupportFormCreated,
                    RelatedEntityId = result.Id,
                    RelatedEntityType = "SupportForm",
                    UserId = null // Broadcast to all admins
                };

                await _notificationService.CreateAsync(notificationDto);

                // Send real-time notification via SignalR
                await _hubContext.Clients.Group("Admins").SendAsync("ReceiveNotification",
                    "Yeni Destek Talebi",
                    $"'{request.Subject}' konulu yeni bir destek talebi oluşturuldu.",
                    result.Id);

                return Ok(new { message = "Destek talebi oluşturuldu", supportFormId = result.Id });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Update support form status (Admin/Manager only)
        /// </summary>
        [HttpPut("{id}/status")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateStatusRequest request)
        {
            var form = await _supportFormService.GetByIdAsync(id);
            if (form is null)
                return NotFound(new { message = "Destek talebi bulunamadı" });

            form.Status = request.Status;
            var result = await _supportFormService.UpdateAsync(id, form);

            if (!string.IsNullOrEmpty(result.Message))
                return BadRequest(new { message = result.Message });

            return Ok(new { message = "Durum güncellendi" });
        }

        private int GetUserIdFromToken()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return !string.IsNullOrEmpty(userIdClaim) && int.TryParse(userIdClaim, out int userId) ? userId : 0;
        }
    }

    public class CreateSupportFormRequest
    {
        public string Subject { get; set; }
        public string Message { get; set; }
    }

    public class UpdateStatusRequest
    {
        public SupportFormStatus Status { get; set; }
    }
}