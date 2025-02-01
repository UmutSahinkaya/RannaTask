using RannaTask.Business.Customers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RannaTask.Business.SupportForms
{
    public interface ISupportFormService
    {
        Task<SupportFormDto?> GetByIdAsync(int id);
        Task<List<SupportFormDto>> GetAllListAsync();
        Task<SupportFormDto> CreateAsync(CreateSupportFormDto request);
        Task<NoContent> UpdateAsync(int id, SupportFormDto request);
        Task<NoContent> DeleteAsync(int id);
    }
}
