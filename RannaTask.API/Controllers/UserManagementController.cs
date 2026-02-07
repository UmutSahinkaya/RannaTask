using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RannaTask.Business.Users;
using RannaTask.Entities.Common;

namespace RannaTask.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")] // Only admins can manage users
    public class UserManagementController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserManagementController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Get all users (Admin only)
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllListAsync();
            return Ok(users);
        }

        /// <summary>
        /// Get user by ID (Admin only)
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _userService.GetByIdAsync(id);
            if (user is null)
                return NotFound(new { message = "Kullanıcı bulunamadı." });
            
            return Ok(user);
        }

        /// <summary>
        /// Update user role (Admin only)
        /// </summary>
        [HttpPut("{id}/role")]
        public async Task<IActionResult> UpdateUserRole(int id, [FromBody] UpdateRoleRequest request)
        {
            var result = await _userService.UpdateRoleAsync(id, request.Role);
            if (!string.IsNullOrEmpty(result.Message))
                return BadRequest(new { message = result.Message });
            
            return Ok(new { message = "Kullanıcı rolü güncellendi." });
        }

        /// <summary>
        /// Toggle user active status (Admin only)
        /// </summary>
        [HttpPut("{id}/toggle-active")]
        public async Task<IActionResult> ToggleUserActive(int id)
        {
            var result = await _userService.ToggleActiveAsync(id);
            if (!string.IsNullOrEmpty(result.Message))
                return BadRequest(new { message = result.Message });
            
            return Ok(new { message = result.Message });
        }

        /// <summary>
        /// Delete user (Admin only)
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var result = await _userService.DeleteAsync(id);
            if (!string.IsNullOrEmpty(result.Message))
                return BadRequest(new { message = result.Message });
            
            return Ok(new { message = "Kullanıcı silindi." });
        }
    }

    public class UpdateRoleRequest
    {
        public UserRole Role { get; set; }
    }
}
