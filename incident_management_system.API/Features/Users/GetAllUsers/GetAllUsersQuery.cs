using MediatR;

namespace incident_management_system.API.Features.Users.GetAllUsers;

public class GetAllUsersQuery : IRequest<List<UserListItem>>
{
}
