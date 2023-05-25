using ProjectAttendance.Host.Application.Users.Commands.Requests;
using ProjectAttendance.Host.Application.Users.Commands.Responses;
using ProjectAttendance.Host.Application.Users.Queries.Responses;

namespace ProjectAttendance.Host.Application.Users.Services
{
    public interface IUserManager
    {
        Task<GetUserQueryResponse> GetUserById(long id);

        Task<CreateUserCommandResponse> CreateUser(CreateUserCommandRequest request);

        Task<UpdateUserCommandResponse> UpdateUser(UpdateUserCommandRequest request);

        Task<AuthenticateCommandResponse> Authenticate(AuthenticateCommandRequest request);
    }
}