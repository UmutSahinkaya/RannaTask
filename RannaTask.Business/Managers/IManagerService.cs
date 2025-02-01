using RannaTask.Business.Customers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RannaTask.Business.Managers
{
    public interface IManagerService
    {
        Task<ManagerDto?> GetByIdAsync(int id);
        Task<List<ManagerDto>> GetAllListAsync();
        Task<CreateManagerResponse> CreateAsync(CreateManagerDto request);
        Task<NoContent> UpdateAsync(int id, ManagerDto request);
        Task<NoContent> DeleteAsync(int id);
    }
}
