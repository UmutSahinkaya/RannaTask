using RannaTask.Entities.Entities;

namespace RannaTask.Business.Customers
{
    public interface ICustomerService
    {
        Task<CustomerDto?> GetByIdAsync(int id);
        Task<List<CustomerDto>> GetAllListAsync();
        Task<CreateCustomerResponse> CreateAsync(CreateCustomerDto request);
        Task<NoContent> UpdateAsync(int id, CustomerDto request);
        Task<NoContent> DeleteAsync(int id);
    }
}
