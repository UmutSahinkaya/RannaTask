using RannaTask.Business.Users;
using RannaTask.Entities.Entities;

namespace RannaTask.Business.Customers
{
    public interface ICustomerService
    {
        Task<Customer> GetByUsernameAndPassword(string username, string password);
        Task<CustomerDto> GetByUsernameAsync(string username);
        Task<CustomerDto> GetByIdAsync(int id);
        Task<List<CustomerDto>> GetAllListAsync();
        Task<CreateCustomerResponse> CreateAsync(CreateCustomerDto request);
        Task<NoContent> UpdateAsync(int id, CustomerDto request);
        Task<NoContent> DeleteByUsernameAsync(string username);
    }
}
