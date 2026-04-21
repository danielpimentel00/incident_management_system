using IMS.Application.Interfaces.Persistance;
using MediatR;

namespace IMS.Application.Features.Users.Queries.GetAllUsers;

public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, List<UserListItem>>
{
    private readonly IUsersRepository _usersRepository;

    public GetAllUsersQueryHandler(IUsersRepository usersRepository)
    {
        _usersRepository = usersRepository;
    }

    public async Task<List<UserListItem>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        var users = await _usersRepository.GetAllAsync();

        var usersDto = users.Select(x => new UserListItem
        {
            Id = x.Id,
            Username = x.Username,
            Email = x.Email
        }).ToList();

        return usersDto;
    }
}