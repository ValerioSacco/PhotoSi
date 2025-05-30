using Microsoft.AspNetCore.Mvc;

namespace PhotoSi.OrdersService.Controllers
{
    [ApiController]
    public class OrdersController : ControllerBase
    {
        public OrdersController()
        {

        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await Task.CompletedTask;
            return Ok();
        }
    }
}
