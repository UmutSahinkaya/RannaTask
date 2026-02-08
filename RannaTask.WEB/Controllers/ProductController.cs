using Microsoft.AspNetCore.Mvc;
using RannaTask.WEB.Common;
using RannaTask.WEB.Models;

namespace RannaTask.WEB.Controllers
{
    public class ProductController : BaseController
    {
        private readonly IWebHostEnvironment _env;

        public ProductController(HttpClient httpClient, IWebHostEnvironment env, IConfiguration configuration) 
            : base(httpClient, configuration)
        {
            _env = env;
        }

        public async Task<IActionResult> Index()
        {
            if (!IsAuthenticated())
                return RedirectToLoginWithMessage();

            var products = await _httpClient.GetFromJsonAsync<List<Product>>("products");
            return View(products);
        }

        [HttpGet]
        public IActionResult CreateProduct()
        {
            if (!IsAuthenticated())
                return RedirectToLoginWithMessage();

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct(Product productViewModel, IFormFile image)
        {
            if (!IsAuthenticated())
                return RedirectToLoginWithMessage();

            await ImageProcess(productViewModel, image);
            var response = await _httpClient.PostAsJsonAsync("products", productViewModel);

            if (response.IsSuccessStatusCode)
                return RedirectToIndexWithSuccess("Ürün başarıyla eklendi.");

            return RedirectToIndexWithError("Ürün eklenemedi. Bir hata meydana geldi.");
        }

        [HttpGet("product/updateproduct/{id}")]
        public async Task<IActionResult> UpdateProduct(int id)
        {
            if (!IsAuthenticated())
                return RedirectToLoginWithMessage();

            var product = await _httpClient.GetFromJsonAsync<Product>($"products/{id}");
            if (product is null)
                return NotFound();

            if (!IsOwnerOrAdmin(product.CreatedBy))
                return RedirectToIndexWithError("Bu ürünü düzenleme yetkiniz yok.");

            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProduct(Product productViewModel, IFormFile image)
        {
            if (!IsAuthenticated())
                return RedirectToLoginWithMessage();

            if (image != null && image.Length > 0)
                await ImageProcess(productViewModel, image);

            var response = await _httpClient.PutAsJsonAsync($"products/{productViewModel.Id}", productViewModel);

            if (response.IsSuccessStatusCode)
                return RedirectToIndexWithSuccess("Ürün başarıyla güncellendi.");

            if (response.StatusCode == System.Net.HttpStatusCode.Forbidden)
                return RedirectToIndexWithError("Bu ürünü güncelleme yetkiniz yok.");

            var errorContent = await response.Content.ReadAsStringAsync();
            return RedirectToIndexWithError($"Ürün güncellenemedi: {errorContent}");
        }

        [HttpGet("deleteproduct/{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            if (!IsAuthenticated())
                return RedirectToLoginWithMessage();

            var product = await _httpClient.GetFromJsonAsync<Product>($"products/{id}");
            if (product is null)
                return RedirectToIndexWithError("Ürün bulunamadı.");

            if (!IsOwnerOrAdmin(product.CreatedBy))
                return RedirectToIndexWithError("Bu ürünü silme yetkiniz yok.");

            var response = await _httpClient.DeleteAsync($"products/{id}");

            if (response.IsSuccessStatusCode)
                return RedirectToIndexWithSuccess("Ürün başarıyla silindi.");

            return RedirectToIndexWithError("Ürün silinemedi. Bir hata meydana geldi.");
        }

        private async Task ImageProcess(Product productViewModel, IFormFile imageFile)
        {
            if (imageFile != null)
            {
                var fileName = Path.GetFileName(imageFile.FileName);
                var filePath = Path.Combine(_env.WebRootPath, "images", fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }
                productViewModel.Image = "/images/" + fileName;
            }
        }
    }
}
