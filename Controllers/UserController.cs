using medium_app_back.Models;
using medium_app_back.Services;
using Microsoft.AspNetCore.Mvc;

namespace medium_app_back.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController(UserService userService) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> CreateUser(User user)
        {
            await userService.AddUserAsync(user);
            return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, user);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound(new { Message = "User not found" });
            }

            return Ok(user);
        }
    }
}
