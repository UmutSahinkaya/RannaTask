using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RannaTask.Business.Products;
using RannaTask.Entities.Common;
using RannaTask.Entities.Entities;
using System.Security.Claims;

namespace RannaTask.API.Controllers
{
    [EnableCors("AllowAllOrigins")]
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // Require authentication
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var products = await _productService.GetAllListAsync();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var product = await _productService.GetByIdAsync(id);
            if (product == null)
                return BadRequest(new { message = "Böyle bir ürün bulunmamakta." });
            return Ok(product);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductDto dto)
        {
            // Add CreatedBy from token
            var userId = GetUserIdFromToken();
            dto.CreatedBy = userId > 0 ? userId : null;

            var product = await _productService.CreateAsync(dto);
            if (product is null)
                return BadRequest(new { message = "Ürün Eklenemedi. Bir sorunla karşılaştık" });
            return Ok(product);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProductAsync(int id, [FromBody] ProductDto dto)
        {
            var existingProduct = await _productService.GetByIdAsync(id);
            if (existingProduct == null)
                return NotFound(new { message = "Ürün bulunamadı" });

            // Check permission: Admin can update all, others only their own
            var userId = GetUserIdFromToken();
            var userRole = GetUserRoleFromToken();

            if (userRole != "Admin" && existingProduct.CreatedBy != userId)
            {
                return StatusCode(403, new { message = "Bu ürünü güncelleme yetkiniz yok. Sadece kendi eklediğiniz ürünleri güncelleyebilirsiniz." });
            }

            var productUpdated = await _productService.UpdateAsync(id, dto);
            if (productUpdated is null || !string.IsNullOrEmpty(productUpdated.Message) && productUpdated.Message.Contains("bulunamadı"))
                return BadRequest(new { message = productUpdated?.Message ?? "Güncelleme başarısız" });

            return Ok(new { message = productUpdated.Message });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var existingProduct = await _productService.GetByIdAsync(id);
            if (existingProduct == null)
                return NotFound(new { message = "Ürün bulunamadı" });

            // Check permission: Admin can delete all, others only their own
            var userId = GetUserIdFromToken();
            var userRole = GetUserRoleFromToken();

            if (userRole != "Admin" && existingProduct.CreatedBy != userId)
            {
                return StatusCode(403, new { message = "Bu ürünü silme yetkiniz yok. Sadece kendi eklediğiniz ürünleri silebilirsiniz." });
            }

            var product = await _productService.DeleteAsync(id);
            if (string.IsNullOrEmpty(product.Message))
                return BadRequest(new { message = "Ürün silinemedi" });

            return Ok(new { message = product.Message });
        }

        private int GetUserIdFromToken()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return !string.IsNullOrEmpty(userIdClaim) && int.TryParse(userIdClaim, out int userId) ? userId : 0;
        }

        private string GetUserRoleFromToken()
        {
            return User.FindFirst(ClaimTypes.Role)?.Value ?? "";
        }
    }
}
