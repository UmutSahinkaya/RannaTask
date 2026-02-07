using RannaTask.Business.Products;
using RannaTask.Entities.Common;
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
        // Authentication
        Task<User> GetByUsernameAndPassword(string username, string password);
        Task<User> GetByEmailAndPassword(string email, string password);

        // User Management
        Task<UserDto?> GetByIdAsync(int id);
        Task<UserDto?> GetByUsernameAsync(string username);
        Task<UserDto?> GetByEmailAsync(string email);
        Task<List<UserDto>> GetAllListAsync();

        // CRUD
        Task<UserDto> CreateAsync(CreateUserDto request);
        Task<NoContent> UpdateAsync(int id, UserDto request);
        Task<NoContent> DeleteAsync(int id);

        // Role Management
        Task<NoContent> UpdateRoleAsync(int id, UserRole role);
        Task<NoContent> ToggleActiveAsync(int id);

        // Password Management
        Task<NoContent> UpdatePasswordAsync(int id, string newPasswordHash);
    }
}
