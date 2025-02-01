using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RannaTask.API.Dtos;
using RannaTask.Business.Customers;
using RannaTask.Business.Helpers;
using RannaTask.Business.Users;
using RannaTask.DAL.Repositories.Customers;
using RannaTask.Entities.Entities;

namespace RannaTask.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;
        private readonly ICustomerService _customerService;

        public AuthController(IUserService userService, ITokenService tokenService, ICustomerService customerService)
        {
            _userService = userService;
            _tokenService = tokenService;
            _customerService = customerService;
        }

        [HttpPost("user/login")]
        public async Task<IActionResult> UserLogin([FromBody] LoginDto request)
        {
            var user= await _userService.GetByUsernameAndPassword(request.Username,request.Password);
            if (user is null)
                return Unauthorized( new {Message="Geçersiz kullanıcı adı veya şifre."});

            var token = _tokenService.GenerateTokenUser(user);
            return Ok(token);
        }
        [HttpPost("customer/login")]
        public async Task<IActionResult> CustomerLogin([FromBody] LoginDto request)
        {
            var customer = await _customerService.GetByUsernameAndPassword(request.Username, request.Password);
            if (customer is null)
                return Unauthorized(new { Message = "Geçersiz kullanıcı adı veya şifre." });

            var token = _tokenService.GenerateTokenCustomer(customer);
            return Ok(token);
        }

        [HttpPost("user/register")]
        public async Task<IActionResult> UserRegister([FromBody] RegisterUserDto request)
        {
            var userDto = await _userService.CreateAsync(new CreateUserDto(request.Username, request.Password, request.Role));
            if (userDto is null)
                return BadRequest(new { message = "Kullanıcı Oluşturulamadı." });
            return Ok(new { message = "Kullanıcı başarıyla oluşturuldu." });
        }
        [HttpPost("customer/register")]
        public async Task<IActionResult> CustomerRegister([FromBody] RegisterCustomerDto request)
        {
            var createCustomerResponse = await _customerService.CreateAsync(new CreateCustomerDto(request.FirstName,request.LastName,request.Email,request.Username,request.Password));
            if (!string.IsNullOrEmpty(createCustomerResponse.Message))
                return BadRequest(new { message = createCustomerResponse.Message });
            return Ok(new { message =createCustomerResponse.Message });
        }

    }
}
