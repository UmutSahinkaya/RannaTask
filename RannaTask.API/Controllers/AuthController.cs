using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RannaTask.API.Dtos;
using RannaTask.Business.Helpers;
using RannaTask.Business.Users;
using RannaTask.Entities.Entities;

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

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto request)
        {
            // var user = _context.Users.FirstOrDefault(u => u.Username == loginRequest.Username && u.Password == loginRequest.Password);
            var user= await _userService.GetByUsernameAndPassword(request.Username,request.Password);
            if (user is null)
                return NotFound("Kullanıcı adı veya Parola hatalı");

            var token = _tokenService.GenerateToken(user);
            return Ok(token);
        }

    }
}
