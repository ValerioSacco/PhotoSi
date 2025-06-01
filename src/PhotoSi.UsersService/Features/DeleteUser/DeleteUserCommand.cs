using MediatR;

namespace PhotoSi.UsersService.Features.DeleteUser
{
    public record DeleteUserCommand(Guid id) : IRequest;
}
