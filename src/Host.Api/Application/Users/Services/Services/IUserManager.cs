using Host.Api.Application.Users.Commands.Requests;
using Host.Api.Application.Users.Commands.Responses;
using Host.Api.Application.Users.Queries.Responses;

namespace Host.Api.Application.Users.Services.Services
{
    public interface IUserManager
    {
        Task<GetUserQueryResponse> GetUserById(long id);

        Task<CreateUserCommandResponse> CreateUser(CreateUserCommandRequest user);

        Task<UpdateUserCommandResponse> UpdateUser(UpdateUserCommandRequest user);

        Task<AuthenticateCommandResponse> Authenticate(AuthenticateCommandRequest auth);
    }
}