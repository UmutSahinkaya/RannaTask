using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using RannaTask.WEB.Common;
using System.Net.Http.Headers;

namespace RannaTask.WEB.Controllers
{
    public class BaseController : Controller
    {
        protected readonly HttpClient _httpClient;

        public BaseController(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("http://localhost:5094/api/");
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            // Her action'dan önce token'ı set et
            SetAuthorizationHeader();
            base.OnActionExecuting(context);
        }

        protected void SetAuthorizationHeader()
        {
            var token = HttpContext.Session.GetJwtToken();
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
        }

        protected bool IsAuthenticated()
        {
            return HttpContext.Session.IsAuthenticated();
        }

        protected bool IsAdmin()
        {
            return HttpContext.Session.IsAdmin();
        }

        protected bool IsOwnerOrAdmin(int? ownerId)
        {
            return HttpContext.Session.IsOwnerOrAdmin(ownerId);
        }

        protected int? GetCurrentUserId()
        {
            return HttpContext.Session.GetUserId();
        }

        protected string? GetCurrentUserRole()
        {
            return HttpContext.Session.GetUserRole();
        }

        protected IActionResult RedirectToLoginWithMessage(string message = null)
        {
            TempData["ErrorMessage"] = message ?? Messages.LoginRequired;
            return RedirectToAction("Login", "Account");
        }

        protected IActionResult RedirectToIndexWithError(string message)
        {
            TempData["ErrorMessage"] = message;
            return RedirectToAction("Index");
        }

        protected IActionResult RedirectToIndexWithSuccess(string message)
        {
            TempData["SuccessMessage"] = message;
            return RedirectToAction("Index");
        }
    }
}
