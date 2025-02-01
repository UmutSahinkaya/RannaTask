using RannaTask.Business.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RannaTask.Business.Users
{
    public interface IUserService
    {
        Task<UserDto?> GetByIdAsync(int id);
        Task<List<UserDto>> GetAllListAsync();
        Task<UserDto> CreateAsync(CreateUserDto request);
        Task<NoContent> UpdateAsync(int id, UserDto request);
        Task<NoContent> DeleteAsync(int id);
    }
}
