using BtkAkademiAIBlog.WebApi.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BtkAkademiAIBlog.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;

        public UsersController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }



        [HttpGet]
        public IActionResult UserList()
        {
            var values = _userManager.Users.ToList();
            return Ok(values);
        }



        [HttpGet("GetUserById")]
        public async Task<IActionResult> GetUserById(string id)
        {
            var values = await _userManager.FindByIdAsync(id);
            return Ok(values);
        }
    }
}
