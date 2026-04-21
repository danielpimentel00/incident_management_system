using MediatR;

namespace IMS.Application.Features.Users.Queries.GetAllUsers;

public class GetAllUsersQuery : IRequest<List<UserListItem>>
{
}
