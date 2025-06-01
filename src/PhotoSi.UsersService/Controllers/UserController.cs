using MediatR;
using Microsoft.AspNetCore.Mvc;
using PhotoSi.UsersService.Features.CreateUser;
using PhotoSi.UsersService.Features.GetUser;
using PhotoSi.UsersService.Features.UpdateUser;

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


        [HttpGet("/users/{id}", Name = "Get one user by id")]
        public async Task<IActionResult> GetById(
            CancellationToken cancellationToken,
            Guid id
        )
        {
            var user = await _mediator.Send(new GetUserQuery(id), cancellationToken);
            return Ok(user);
        }


        [HttpPost("/users", Name = "Create new user")]
        public async Task<IActionResult> Create(
            CancellationToken cancellationToken, 
            [FromBody] UpdateUserCommand command
        )
        {
            var userId = await _mediator.Send(command, cancellationToken);
            return CreatedAtRoute(
                "Get one user by id",
                new { id = userId },
                new { userId = userId }
            );
        }
    }
}
