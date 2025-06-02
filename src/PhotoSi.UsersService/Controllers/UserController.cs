using MediatR;
using Microsoft.AspNetCore.Mvc;
using PhotoSi.UsersService.Features.CreateUser;
using PhotoSi.UsersService.Features.DeleteUser;
using PhotoSi.UsersService.Features.GetUser;
using PhotoSi.UsersService.Features.ListUsers;
using PhotoSi.UsersService.Features.UpdateUser;
using Swashbuckle.AspNetCore.Annotations;

namespace PhotoSi.UsersService.Controllers
{
    [ApiController]
    [Produces("application/json")]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [SwaggerOperation(Summary = "Get user by id", Description = "Retrieves the details of an user given its unique id.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Returns the product details", typeof(GetUserResponse))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Product does not exists")]
        [HttpGet("/users/{id}", Name = "Get one user by id")]
        public async Task<IActionResult> GetById(
            CancellationToken cancellationToken,
            Guid id
        )
        {
            var user = await _mediator.Send(new GetUserQuery(id), cancellationToken);
            return Ok(user);
        }

        [SwaggerOperation(Summary = "Get list of users", Description = "Retrieves the list of users with pagination")]
        [SwaggerResponse(StatusCodes.Status200OK, "Returns the product details", typeof(ListUsersResponse))]
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


        [SwaggerOperation(Summary = "Create new user", Description = "Creates a new user and returns its unique id.")]
        [SwaggerResponse(StatusCodes.Status201Created, "User created successfully, returns the new user id", typeof(Guid))]
        [SwaggerResponse(StatusCodes.Status422UnprocessableEntity, "Input data are not accetable")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "User created with a not existing shipment address")]
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

        [SwaggerOperation(Summary = "Update existing user by id", Description = "Updates an existing user and returns its unique id.")]
        [SwaggerResponse(StatusCodes.Status202Accepted, "User updates accepted successfully, returns the user id updated", typeof(Guid))]
        [SwaggerResponse(StatusCodes.Status422UnprocessableEntity, "Input data are not accetable")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "User updated with a not existing shipment address")]
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


        [SwaggerOperation(Summary = "Delete existing user by id", Description = "Deletes an existing user.")]
        [SwaggerResponse(StatusCodes.Status204NoContent, "User deleted successfully")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "User does not exists")]
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
