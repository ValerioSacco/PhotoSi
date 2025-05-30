using Microsoft.AspNetCore.Mvc;

namespace PhotoSi.UsersService.Controllers
{
    [ApiController]
    public class UserController : ControllerBase
    {
        public UserController()
        {

        }

        [HttpPost("/users", Name = "Create new user")]
        public async Task<IActionResult> Create()
        {
            return Ok();
        }
    }
}
