using RannaTask.Business.Products;
using RannaTask.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RannaTask.Business.Users
{
    public interface IUserService
    {
        Task<User> GetByUsernameAndPassword(string username, string password);
        Task<UserDto?> GetByUsernameAsync(string username);
        Task<List<UserDto>> GetAllListAsync();
        Task<UserDto> CreateAsync(CreateUserDto request);
        Task<NoContent> UpdateAsync(string username, UserDto request);
        Task<NoContent> DeleteByUsernameAsync(string username);
    }
}
