using Host.Api.Application.Users.Commands.Requests;
using Host.Api.Application.Users.Commands.Responses;
using Host.Api.Application.Users.Queries.Responses;

namespace Host.Api.Application.Users.Services.Services
{
    public class UserManager : IUserManager
    {
        public Task<AuthenticateCommandResponse> Authenticate(AuthenticateCommandRequest auth)
        {
            throw new NotImplementedException();
        }

        public Task<CreateUserCommandResponse> CreateUser(CreateUserCommandRequest user)
        {
            throw new NotImplementedException();
        }

        public Task<GetUserQueryResponse> GetUserById(long id)
        {
            throw new NotImplementedException();
        }

        public Task<UpdateUserCommandResponse> UpdateUser(UpdateUserCommandRequest user)
        {
            throw new NotImplementedException();
        }
    }
}
