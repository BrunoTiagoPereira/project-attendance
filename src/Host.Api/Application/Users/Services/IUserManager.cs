using Host.Api.Application.Users.Commands.Requests;
using Host.Api.Application.Users.Commands.Responses;
using Host.Api.Application.Users.Queries.Responses;

namespace Host.Api.Application.Users.Services
{
    public interface IUserManager
    {
        Task<GetUserQueryResponse> GetUserById(long id);

        Task<CreateUserCommandResponse> CreateUser(CreateUserCommandRequest request);

        Task<UpdateUserCommandResponse> UpdateUser(UpdateUserCommandRequest request);

        Task<AuthenticateCommandResponse> Authenticate(AuthenticateCommandRequest request);
    }
}