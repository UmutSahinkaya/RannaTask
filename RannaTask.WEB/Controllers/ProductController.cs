using Microsoft.AspNetCore.Mvc;
using RannaTask.WEB.Models;

namespace RannaTask.WEB.Controllers
{
    public class ProductController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly IWebHostEnvironment _env;

        public ProductController(HttpClient httpClient, IWebHostEnvironment env)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new System.Uri("http://localhost:5094/api/");
            _env = env;
        }

        public async Task<IActionResult> Index()
        {
            var products = await _httpClient.GetFromJsonAsync<List<Product>>("products");
            return View(products);
        }
        [HttpGet]
        public IActionResult CreateProduct() 
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateProduct(Product productViewModel, IFormFile image)
        {
            await ImageProcess(productViewModel, image);
            var response = await _httpClient.PostAsJsonAsync("products", productViewModel);
            if (response.IsSuccessStatusCode)
                TempData["SuccessMessage"] = "ürün başarıyla eklendi.";
            else
                TempData["ErrorMessage"] = "Ürün eklenemedi.Bir Hata meydana geldi";
            return Redirect("Index");
        }
        [HttpGet("product/updateproduct/{id}")]
        public async Task<IActionResult> UpdateProduct(int id)
        {
            var product= await _httpClient.GetFromJsonAsync<Product>($"products/{id}");
            if (product is null)
                return NotFound();
            return View(product);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateProduct(Product productViewModel, IFormFile image)
        {
            await ImageProcess(productViewModel, image);
                            
            var response = await _httpClient.PutAsJsonAsync($"products/{productViewModel.Id}",productViewModel);
            if (response.IsSuccessStatusCode)
                TempData["SuccessMessage"] = "ürün başarıyla gğncellendi.";
            else
                TempData["ErrorMessage"] = "Ürün güncellenemedi.Bir Hata meydana geldi";
            return RedirectToAction("Index");
        }

        [HttpGet("deleteproduct/{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var response = await _httpClient.DeleteAsync($"products/{id}");
            if (response.IsSuccessStatusCode)
                TempData["SuccessMessage"] = "ürün başarıyla silindi.";
            else
                TempData["ErrorMessage"] = "Ürün silinmedi.Bir Hata meydana geldi";
            return RedirectToAction("Index");
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
