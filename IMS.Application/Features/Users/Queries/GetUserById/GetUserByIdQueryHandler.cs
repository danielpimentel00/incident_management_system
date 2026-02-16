using IMS.Application.Interfaces.Persistance;
using MediatR;

namespace IMS.Application.Features.Users.Queries.GetUserById;

public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, UserDetails?>
{
    private readonly IUsersRepository _usersRepository;

    public GetUserByIdQueryHandler(IUsersRepository usersRepository)
    {
        _usersRepository = usersRepository;
    }

    public async Task<UserDetails?> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await _usersRepository.GetUserByIdAsync(request.Id);

        if (user == null)
        {
            return null;
        }

        var userDto = new UserDetails
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email,
        };

        return userDto;
    }
}
