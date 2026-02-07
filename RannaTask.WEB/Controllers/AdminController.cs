using Microsoft.AspNetCore.Mvc;
using RannaTask.WEB.Common;
using RannaTask.WEB.Models;

namespace RannaTask.WEB.Controllers
{
    public class AdminController : BaseController
    {
        public AdminController(HttpClient httpClient) : base(httpClient)
        {
        }

        public IActionResult Index()
        {
            if (!IsAuthenticated())
                return RedirectToLoginWithMessage();

            if (!IsAdmin())
                return RedirectToAction("Index", "Home");

            return View();
        }

        public async Task<IActionResult> Users()
        {
            if (!IsAuthenticated() || !IsAdmin())
                return RedirectToLoginWithMessage(Messages.UnauthorizedAccess);

            try
            {
                var response = await _httpClient.GetAsync("usermanagement");

                if (response.IsSuccessStatusCode)
                {
                    var users = await response.Content.ReadFromJsonAsync<List<AdminUserViewModel>>();
                    return View(users ?? new List<AdminUserViewModel>());
                }

                TempData["ErrorMessage"] = "Kullanıcılar yüklenirken bir hata oluştu.";
                return View(new List<AdminUserViewModel>());
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Bir hata oluştu: " + ex.Message;
                return View(new List<AdminUserViewModel>());
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateUserRole(int userId, int role)
        {
            if (!IsAuthenticated() || !IsAdmin())
                return Json(new { success = false, message = Messages.UnauthorizedAccess });

            try
            {
                var requestData = new { role = role };
                var response = await _httpClient.PutAsJsonAsync($"usermanagement/{userId}/role", requestData);

                if (response.IsSuccessStatusCode)
                    return Json(new { success = true, message = "Kullanıcı rolü güncellendi" });

                return Json(new { success = false, message = "Güncelleme başarısız" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> ToggleUserActive(int userId)
        {
            if (!IsAuthenticated() || !IsAdmin())
                return Json(new { success = false, message = Messages.UnauthorizedAccess });

            try
            {
                var response = await _httpClient.PutAsync($"usermanagement/{userId}/toggle-active", null);

                if (response.IsSuccessStatusCode)
                    return Json(new { success = true, message = "Kullanıcı durumu güncellendi" });

                return Json(new { success = false, message = "Güncelleme başarısız" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        public async Task<IActionResult> SupportForms()
        {
            if (!IsAuthenticated())
                return RedirectToLoginWithMessage();

            if (!IsAdmin())
                return RedirectToAction("Index", "Home");

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
                    return RedirectToLoginWithMessage("Oturum süreniz dolmuş. Lütfen tekrar giriş yapın.");
                }

                TempData["ErrorMessage"] = "Destek formları yüklenirken bir hata oluştu.";
                return View(new List<AdminSupportFormViewModel>());
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Bir hata oluştu: " + ex.Message;
                return View(new List<AdminSupportFormViewModel>());
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateStatus(int id, int status, string reason)
        {
            if (!IsAuthenticated() || !IsAdmin())
                return Json(new { success = false, message = Messages.UnauthorizedAccess });

            try
            {
                var requestData = new { status = status, reason = reason ?? "" };
                var response = await _httpClient.PutAsJsonAsync($"supportform/{id}/status", requestData);

                if (response.IsSuccessStatusCode)
                    return Json(new { success = true, message = "Durum güncellendi ve müşteri bilgilendirildi" });

                return Json(new { success = false, message = "Güncelleme başarısız" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}
