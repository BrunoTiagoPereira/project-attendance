using ProjectAttendance.Host.Application.Users.Commands.Requests;
using ProjectAttendance.Host.Application.Users.Commands.Responses;
using ProjectAttendance.Host.Application.Users.Queries.Requests;
using ProjectAttendance.Host.Application.Users.Queries.Responses;

namespace ProjectAttendance.Host.Application.Users.Services
{
    public interface IUserManager
    {
        Task<GetUserQueryResponse> GetUser(GetUserQueryRequest request);

        Task<CreateUserCommandResponse> CreateUserAsync(CreateUserCommandRequest request);

        Task<UpdateUserCommandResponse> UpdateUserAsync(UpdateUserCommandRequest request);

        Task<AuthenticateQueryResponse> Authenticate(AuthenticateQueryRequest request);
    }
}