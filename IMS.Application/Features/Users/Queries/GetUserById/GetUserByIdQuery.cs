using MediatR;

namespace IMS.Application.Features.Users.Queries.GetUserById;

public class GetUserByIdQuery(int id) : IRequest<UserDetails?>
{
    public int Id { get; } = id;
}
