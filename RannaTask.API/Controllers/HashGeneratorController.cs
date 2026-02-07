using Microsoft.AspNetCore.Mvc;
using RannaTask.Business.Helpers;

namespace RannaTask.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HashGeneratorController : ControllerBase
    {
        [HttpGet("generate")]
        public IActionResult GenerateHashes()
        {
            var result = new
            {
                admin = new { 
                    username = "admin", 
                    password = "admin123", 
                    hash = PasswordHasher.HashPassword("admin123") 
                },
                paneluser = new { 
                    username = "paneluser", 
                    password = "panel123", 
                    hash = PasswordHasher.HashPassword("panel123") 
                },
                testuser = new { 
                    username = "testuser", 
                    password = "test123", 
                    hash = PasswordHasher.HashPassword("test123") 
                },
                demouser = new { 
                    username = "demouser", 
                    password = "demo123", 
                    hash = PasswordHasher.HashPassword("demo123") 
                }
            };

            return Ok(result);
        }

        [HttpPost("verify")]
        public IActionResult VerifyPassword([FromBody] VerifyRequest request)
        {
            var isValid = PasswordHasher.VerifyPassword(request.Password, request.Hash);
            return Ok(new { isValid, password = request.Password, hash = request.Hash });
        }
    }

    public class VerifyRequest
    {
        public string Password { get; set; }
        public string Hash { get; set; }
    }
}
