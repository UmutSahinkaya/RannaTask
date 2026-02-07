using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RannaTask.API.Dtos;
using RannaTask.Business.Helpers;
using RannaTask.Business.Users;
using RannaTask.Entities.Common;

namespace RannaTask.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;

        public AuthController(IUserService userService, ITokenService tokenService)
        {
            _userService = userService;
            _tokenService = tokenService;
        }

        /// <summary>
        /// Single login endpoint - works with username or email
        /// </summary>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto request)
        {
            // Try username first
            var user = await _userService.GetByUsernameAndPassword(request.Username, request.Password);

            // If not found, try email
            if (user is null)
            {
                user = await _userService.GetByEmailAndPassword(request.Username, request.Password);
            }

            if (user is null)
                return Unauthorized(new { message = "Geçersiz kullanıcı adı/email veya şifre." });

            if (!user.IsActive)
                return Unauthorized(new { message = "Hesabınız aktif değil. Lütfen yönetici ile iletişime geçin." });

            var token = _tokenService.GenerateTokenUser(user);

            return Ok(new 
            { 
                token = token,
                user = new
                {
                    id = user.Id,
                    username = user.Username,
                    email = user.Email,
                    firstName = user.FirstName,
                    lastName = user.LastName,
                    role = user.Role.ToString()
                }
            });
        }

        /// <summary>
        /// Public registration - always creates Customer role
        /// </summary>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterCustomerDto request)
        {
            try
            {
                var createUserDto = new CreateUserDto(
                    username: request.Username,
                    email: request.Email,
                    password: request.Password,
                    firstName: request.FirstName,
                    lastName: request.LastName,
                    role: UserRole.Customer // Always Customer for public registration
                );

                var userDto = await _userService.CreateAsync(createUserDto);

                return Ok(new 
                { 
                    message = "Kayıt başarılı! Giriş yapabilirsiniz.",
                    userId = userDto.Id
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Reset password endpoint
        /// </summary>
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
        {
            try
            {
                // Email ile kullanıcı bul (UserDto değil User entity lazım)
                var userDto = await _userService.GetByEmailAsync(request.Email);

                if (userDto == null)
                    return BadRequest(new { message = "Bu email adresiyle kayıtlı kullanıcı bulunamadı." });

                // Şifreyi hashle ve güncelle
                var hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);

                await _userService.UpdatePasswordAsync(userDto.Id, hashedPassword);

                return Ok(new { message = "Şifreniz başarıyla değiştirildi." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Şifre sıfırlama başarısız: " + ex.Message });
            }
        }
    }

    public class ResetPasswordRequest
    {
        public string Email { get; set; }
        public string NewPassword { get; set; }
    }
}
