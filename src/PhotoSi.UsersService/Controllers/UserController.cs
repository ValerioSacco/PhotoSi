using MediatR;
using Microsoft.AspNetCore.Mvc;
using PhotoSi.UsersService.Features.CreateUser;

namespace PhotoSi.UsersService.Controllers
{
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("/users", Name = "Create new user")]
        public async Task<IActionResult> Create(
            CancellationToken cancellationToken, 
            [FromBody] CreateUserCommand command
        )
        {
            var userId = await _mediator.Send(command, cancellationToken);
            return CreatedAtRoute(
                "Get one product by id",
                new { id = userId },
                new { userId = userId }
            );
        }
    }
}
