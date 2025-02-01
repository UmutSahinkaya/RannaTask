using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RannaTask.Business.SupportForms;
using System.Security.Claims;

namespace RannaTask.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SupportFormController : ControllerBase
    {
        private readonly ISupportFormService _supportFormService;
        private readonly IHttpContextAccessor _contextAccessor;

        public SupportFormController(ISupportFormService supportFormService, IHttpContextAccessor contextAccessor)
        {
            _supportFormService = supportFormService;
            _contextAccessor = contextAccessor;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateSupportForm([FromBody] CreateSupportFormDto request)
        {
            if (!IsCustomer())
                return Unauthorized(new { message = "Sadece müşteriler destek formu oluşturabilir." });
            var customerId=GetCustomerIdFromToken();
            var form = await _supportFormService.CreateAsync(new CreateSupportFormDto(request.Subject, request.Message, customerId));
            return Ok(new { message = "Destek formu başarıyla oluşturulmuştur." });
        }

        [Authorize(Roles ="PanelUser,Admin")]
        [HttpGet("all-forms")]
        public async Task<IActionResult> GetAllForms()
        {
            var forms= await _supportFormService.GetAllListAsync();
            return Ok(forms);
        }
            

        private bool IsCustomer()
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;
            var isCustomerClaim = claimsIdentity?.FindFirst("IsCustomer");
            return isCustomerClaim !=null && isCustomerClaim.Value == "true";
        }

        private int GetCustomerIdFromToken()
        {
            var claimsIdentity=User.Identity as ClaimsIdentity;
            var customerIdClaim = claimsIdentity?.FindFirst(ClaimTypes.NameIdentifier);
            return customerIdClaim != null ? int.Parse(customerIdClaim.Value) : 0;
        }
    }
}
