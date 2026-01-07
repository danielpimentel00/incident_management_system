using MediatR;

namespace incident_management_system.API.Features.Users.GetUserById;

public class GetUserByIdQuery(int id) : IRequest<UserDetails?>
{
    public int Id { get; } = id;
}
