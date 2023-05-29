using ProjectAttendance.Host.Application.Projects.Commands.Requests;
using ProjectAttendance.Host.Application.Projects.Queries.Requests;
using ProjectAttendance.Host.Application.Projects.Queries.Responses;

namespace ProjectAttendance.Host.Application.Projects.Services
{
    public interface IProjectManager
    {
        Task<AttendToProjectCommandResponse> AttendToProjectAsync(AttendToProjectCommandRequest request);
        Task<CreateProjectCommandResponse> CreateProjectAsync(CreateProjectCommandRequest request);
        Task<GetProjectQueryResponse> GetProjectAsync(GetProjectQueryRequest request);
    }
}
