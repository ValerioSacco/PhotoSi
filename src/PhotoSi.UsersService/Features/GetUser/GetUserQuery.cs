using MediatR;

namespace PhotoSi.UsersService.Features.GetUser
{
    public record GetUserQuery(Guid id) : IRequest<GetUserResponse>;
}
