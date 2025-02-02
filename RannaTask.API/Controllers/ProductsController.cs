using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RannaTask.Business.Products;
using RannaTask.Entities.Entities;

namespace RannaTask.API.Controllers
{
    [EnableCors("AllowAllOrigins")]
    [Route("api/[controller]")]
    [ApiController]
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
            var product = await _productService.CreateAsync(dto);
            if (product is null)
                return BadRequest(new { message = "Ürün Eklenemedi. Bir sorunla karşılaştık" });
            return Ok(product);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProductAsync(int id, [FromBody] ProductDto dto)
        {
            var productUpdated = await _productService.UpdateAsync(id, dto);
            if (productUpdated is null)
                return BadRequest(new { message = productUpdated?.Message });
            return Ok(new {productUpdated.Message});
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _productService.DeleteAsync(id);
            if (string.IsNullOrEmpty(product.Message))
                return BadRequest(new { message = "Böyle bir ürün bulunmamakta." });
            return Ok(new {product.Message});
        }


    }
}
