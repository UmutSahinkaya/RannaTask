using Microsoft.AspNetCore.Mvc;
using RannaTask.WEB.Common;
using RannaTask.WEB.Models;

namespace RannaTask.WEB.Controllers
{
    public class AccountController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public AccountController(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;

            // API Base URL'yi configuration'dan al
            var apiBaseUrl = _configuration["ApiSettings:BaseUrl"] ?? "http://localhost:5094/api/";
            _httpClient.BaseAddress = new Uri(apiBaseUrl);
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

            // Şifre deneme sayacını kontrol et
            var loginAttemptKey = $"LoginAttempt_{model.Username}";
            var attemptCount = HttpContext.Session.GetInt32(loginAttemptKey) ?? 0;

            if (attemptCount >= 3)
            {
                ModelState.AddModelError(string.Empty, "Çok fazla başarısız deneme! Lütfen birkaç dakika sonra tekrar deneyin.");
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

                    HttpContext.Session.SetString(SessionKeys.JWTToken, loginResponse.Token);
                    HttpContext.Session.SetString(SessionKeys.Username, loginResponse.User.Username);
                    HttpContext.Session.SetString(SessionKeys.UserRole, loginResponse.User.Role);
                    HttpContext.Session.SetInt32(SessionKeys.UserId, loginResponse.User.Id);

                    // Başarılı giriş - Sayacı sıfırla
                    HttpContext.Session.Remove(loginAttemptKey);

                    TempData["SuccessMessage"] = Messages.LoginSuccess;
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    // Başarısız giriş - Sayacı artır
                    attemptCount++;
                    HttpContext.Session.SetInt32(loginAttemptKey, attemptCount);

                    var remainingAttempts = 3 - attemptCount;
                    if (remainingAttempts > 0)
                    {
                        ModelState.AddModelError(string.Empty, 
                            $"Kullanıcı adı veya şifre hatalı. Kalan deneme hakkı: {remainingAttempts}");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, 
                            "Çok fazla başarısız deneme! Hesabınız geçici olarak kilitlendi.");
                    }

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
            TempData["SuccessMessage"] = Messages.LogoutSuccess;
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

    // Response models
    public class LoginResponse
    {
        public string Token { get; set; }
        public UserInfo User { get; set; }
    }

    public class UserInfo
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Role { get; set; }
    }
}
