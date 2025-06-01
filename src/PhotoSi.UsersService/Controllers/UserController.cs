using MediatR;
using Microsoft.AspNetCore.Mvc;
using PhotoSi.UsersService.Features.CreateUser;
using PhotoSi.UsersService.Features.DeleteUser;
using PhotoSi.UsersService.Features.GetUser;
using PhotoSi.UsersService.Features.ListUsers;
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

        [HttpGet("/users", Name = "List all users")]
        public async Task<IActionResult> List(
            CancellationToken cancellationToken,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10
        )
        {
            var users = await _mediator.Send(new ListUsersQuery(pageNumber, pageSize), cancellationToken);
            return Ok(users);
        }

        [HttpPost("/users", Name = "Create new user")]
        public async Task<IActionResult> Create(
            CancellationToken cancellationToken,
            [FromBody] CreateUserCommand command
        )
        {
            var userId = await _mediator.Send(command, cancellationToken);
            return CreatedAtRoute(
                "Get one user by id",
                new { id = userId },
                new { userId = userId }
            );
        }

        [HttpPut("/users/{id}", Name = "Update one user")]
        public async Task<IActionResult> Update(
            CancellationToken cancellationToken,
            Guid id,
            [FromBody] UpdateUserCommand command
        )
        {
            var userId = await _mediator.Send(command with { id = id }, cancellationToken);
            return AcceptedAtRoute(
                "Get one user by id",
                new { id = userId },
                new { userId = userId }
            );
        }

        [HttpDelete("/users/{id}", Name = "Delete one user")]
        public async Task<IActionResult> Delete(
            CancellationToken cancellationToken,
            Guid id
        )
        {
            await _mediator.Send(new DeleteUserCommand(id), cancellationToken);
            return NoContent();
        }
    }
}
