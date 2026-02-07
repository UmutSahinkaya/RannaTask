using Microsoft.AspNetCore.Mvc;
using RannaTask.WEB.Models;

namespace RannaTask.WEB.Controllers
{
    public class AccountController : Controller
    {
        private readonly HttpClient _httpClient;

        public AccountController(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new System.Uri("http://localhost:5094/api/");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var loginData = new
                {
                    username = model.Username,
                    password = model.Password
                };

                // Use single login endpoint
                var response = await _httpClient.PostAsJsonAsync("auth/login", loginData);

                if (response.IsSuccessStatusCode)
                {
                    var loginResponse = await response.Content.ReadFromJsonAsync<LoginResponse>();

                    HttpContext.Session.SetString("JWTToken", loginResponse.Token);
                    HttpContext.Session.SetString("Username", loginResponse.User.Username);
                    HttpContext.Session.SetString("UserRole", loginResponse.User.Role);

                    TempData["SuccessMessage"] = "Giriş başarılı!";
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    var errorResponse = await response.Content.ReadAsStringAsync();
                    ModelState.AddModelError(string.Empty, "Kullanıcı adı veya şifre hatalı.");
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Bir hata oluştu: " + ex.Message);
                return View(model);
            }
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var registerData = new
                {
                    firstName = model.FirstName,
                    lastName = model.LastName,
                    email = model.Email,
                    username = model.Username,
                    password = model.Password
                };

                // Use single register endpoint
                var response = await _httpClient.PostAsJsonAsync("auth/register", registerData);

                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "Kayıt başarılı! Giriş yapabilirsiniz.";
                    return RedirectToAction("Login");
                }
                else
                {
                    var errorResponse = await response.Content.ReadAsStringAsync();
                    ModelState.AddModelError(string.Empty, "Kayıt işlemi başarısız oldu.");
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Bir hata oluştu: " + ex.Message);
                return View(model);
            }
        }

        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            TempData["SuccessMessage"] = "Başarıyla çıkış yaptınız.";
            return RedirectToAction("Login");
        }

        // GET: Account/ForgotPassword
        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        // POST: Account/ForgotPassword
        [HttpPost]
        public IActionResult ForgotPassword(ForgotPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Basit kod: Email'i session'a kaydet, kod üret
            var resetCode = new Random().Next(100000, 999999).ToString();
            HttpContext.Session.SetString("ResetEmail", model.Email);
            HttpContext.Session.SetString("ResetCode", resetCode);

            // Gerçek uygulamada email gönderilir, şimdilik TempData'da göster
            TempData["ResetCode"] = resetCode;
            TempData["SuccessMessage"] = $"Şifre sıfırlama kodunuz: {resetCode} (Normalde email'inize gönderilir)";

            return RedirectToAction("ResetPassword");
        }

        // GET: Account/ResetPassword
        [HttpGet]
        public IActionResult ResetPassword()
        {
            var email = HttpContext.Session.GetString("ResetEmail");
            if (string.IsNullOrEmpty(email))
            {
                return RedirectToAction("ForgotPassword");
            }

            return View(new ResetPasswordViewModel { Email = email });
        }

        // POST: Account/ResetPassword
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var sessionCode = HttpContext.Session.GetString("ResetCode");
            var sessionEmail = HttpContext.Session.GetString("ResetEmail");

            if (sessionCode != model.ResetCode || sessionEmail != model.Email)
            {
                TempData["ErrorMessage"] = "Geçersiz kod veya email!";
                return View(model);
            }

            // API'ye şifre güncelleme isteği gönder
            var requestData = new { email = model.Email, newPassword = model.NewPassword };
            var response = await _httpClient.PostAsJsonAsync("auth/reset-password", requestData);

            if (response.IsSuccessStatusCode)
            {
                HttpContext.Session.Remove("ResetCode");
                HttpContext.Session.Remove("ResetEmail");
                TempData["SuccessMessage"] = "Şifreniz başarıyla değiştirildi! Giriş yapabilirsiniz.";
                return RedirectToAction("Login");
            }
            else
            {
                TempData["ErrorMessage"] = "Şifre değiştirilemedi. Lütfen tekrar deneyin.";
                return View(model);
            }
        }
    }
}
