using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RannaTask.Business.Products
{
    public interface IProductService
    {
        Task<ProductDto?> GetByIdAsync(int id);
        Task<List<ProductDto>> GetAllListAsync();
        Task<CreateProductResponse> CreateAsync(CreateProductDto request);
        Task<NoContent> UpdateAsync(int id, ProductDto request);
        Task<NoContent> DeleteAsync(int id);
    }
}
