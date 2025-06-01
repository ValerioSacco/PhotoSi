using MediatR;

namespace PhotoSi.UsersService.Features.ListUsers
{
    public record ListUsersQuery(int pageNumber, int pageSize) : IRequest<ListUsersResponse>;
}
