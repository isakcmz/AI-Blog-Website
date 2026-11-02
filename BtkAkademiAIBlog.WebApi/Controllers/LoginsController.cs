using BtkAkademiAIBlog.WebApi.Dtos.LoginDtos;
using BtkAkademiAIBlog.WebApi.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BtkAkademiAIBlog.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginsController : ControllerBase
    {
        private readonly SignInManager<AppUser> _signInManager;

        public LoginsController(SignInManager<AppUser> signInManager)
        {
            _signInManager = signInManager;
        }






        [HttpPost]
        public async Task<IActionResult> UserLogin(UserLoginDto userLoginDto)
        {
            var result = await _signInManager.PasswordSignInAsync(userLoginDto.Username, userLoginDto.Password, false, false);

            if(result.Succeeded)
            {
                return Ok("Giriş işlemi başarılı. Kullanıcı: " + userLoginDto.Username);
            }
            else
            {
                return BadRequest("Giriş işlemi başarısız. Kullanıcı adı veya şifre hatalı.");
            }
        }
    }
}
