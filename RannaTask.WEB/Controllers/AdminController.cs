using Microsoft.AspNetCore.Mvc;
using RannaTask.WEB.Models;
using System.Net.Http.Headers;

namespace RannaTask.WEB.Controllers
{
    public class AdminController : Controller
    {
        private readonly HttpClient _httpClient;

        public AdminController(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new System.Uri("http://localhost:5094/api/");
        }

        private void SetAuthorizationHeader()
        {
            var token = HttpContext.Session.GetString("JWTToken");
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
        }

        private bool IsAuthenticated()
        {
            return !string.IsNullOrEmpty(HttpContext.Session.GetString("JWTToken"));
        }

        private bool IsAdmin()
        {
            var role = HttpContext.Session.GetString("UserRole");
            return role == "Admin" || role == "Manager";
        }

        // GET: Admin/SupportForms
        public async Task<IActionResult> SupportForms()
        {
            if (!IsAuthenticated())
            {
                TempData["ErrorMessage"] = "Lütfen önce giriş yapın.";
                return RedirectToAction("Login", "Account");
            }

            if (!IsAdmin())
            {
                TempData["ErrorMessage"] = "Bu sayfaya erişim yetkiniz yok.";
                return RedirectToAction("Index", "Home");
            }

            SetAuthorizationHeader();

            try
            {
                var response = await _httpClient.GetAsync("supportform");

                if (response.IsSuccessStatusCode)
                {
                    var forms = await response.Content.ReadFromJsonAsync<List<AdminSupportFormViewModel>>();
                    return View(forms ?? new List<AdminSupportFormViewModel>());
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    TempData["ErrorMessage"] = "Oturum süreniz dolmuş. Lütfen tekrar giriş yapın.";
                    return RedirectToAction("Login", "Account");
                }
                else
                {
                    TempData["ErrorMessage"] = "Destek formları yüklenirken bir hata oluştu.";
                    return View(new List<AdminSupportFormViewModel>());
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Bir hata oluştu: " + ex.Message;
                return View(new List<AdminSupportFormViewModel>());
            }
        }

        // POST: Admin/UpdateStatus
        [HttpPost]
        public async Task<IActionResult> UpdateStatus(int id, int status, string reason)
        {
            if (!IsAuthenticated() || !IsAdmin())
            {
                return Json(new { success = false, message = "Yetkiniz yok" });
            }

            SetAuthorizationHeader();

            try
            {
                var requestData = new { status = status, reason = reason ?? "" };
                var response = await _httpClient.PutAsJsonAsync($"supportform/{id}/status", requestData);

                if (response.IsSuccessStatusCode)
                {
                    return Json(new { success = true, message = "Durum güncellendi ve müşteri bilgilendirildi" });
                }
                else
                {
                    return Json(new { success = false, message = "Güncelleme başarısız" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}
