using Microsoft.AspNetCore.Mvc;
using RannaTask.WEB.Models;
using System.Net.Http.Headers;

namespace RannaTask.WEB.Controllers
{
    public class SupportFormController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public SupportFormController(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;

            // API Base URL'yi configuration'dan al
            var apiBaseUrl = _configuration["ApiSettings:BaseUrl"] ?? "http://localhost:5094/api/";
            _httpClient.BaseAddress = new Uri(apiBaseUrl);
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

        // GET: SupportForm
        public async Task<IActionResult> Index()
        {
            if (!IsAuthenticated())
            {
                TempData["ErrorMessage"] = "Lütfen önce giriş yapın.";
                return RedirectToAction("Login", "Account");
            }

            SetAuthorizationHeader();

            try
            {
                var response = await _httpClient.GetAsync("supportform/my-forms");
                
                if (response.IsSuccessStatusCode)
                {
                    var forms = await response.Content.ReadFromJsonAsync<List<SupportFormViewModel>>();
                    return View(forms ?? new List<SupportFormViewModel>());
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    TempData["ErrorMessage"] = "Oturum süreniz dolmuş. Lütfen tekrar giriş yapın.";
                    return RedirectToAction("Login", "Account");
                }
                else
                {
                    TempData["ErrorMessage"] = "Destek formları yüklenirken bir hata oluştu.";
                    return View(new List<SupportFormViewModel>());
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Bir hata oluştu: " + ex.Message;
                return View(new List<SupportFormViewModel>());
            }
        }

        // GET: SupportForm/Create
        [HttpGet]
        public IActionResult Create()
        {
            if (!IsAuthenticated())
            {
                TempData["ErrorMessage"] = "Lütfen önce giriş yapın.";
                return RedirectToAction("Login", "Account");
            }

            return View();
        }

        // POST: SupportForm/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateSupportFormViewModel model)
        {
            if (!IsAuthenticated())
            {
                TempData["ErrorMessage"] = "Lütfen önce giriş yapın.";
                return RedirectToAction("Login", "Account");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            SetAuthorizationHeader();

            try
            {
                var requestData = new
                {
                    subject = model.Subject,
                    message = model.Message
                };

                var response = await _httpClient.PostAsJsonAsync("supportform", requestData);

                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "Destek talebiniz başarıyla oluşturuldu. En kısa sürede size dönüş yapılacaktır.";
                    return RedirectToAction("Index");
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    TempData["ErrorMessage"] = "Destek talebi oluşturulurken bir hata oluştu.";
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Bir hata oluştu: " + ex.Message;
                return View(model);
            }
        }
    }
}
