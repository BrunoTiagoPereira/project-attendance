using ProjectAttendance.Host.Application.Projects.Commands.Requests;

namespace ProjectAttendance.Host.Application.Projects.Services
{
    public interface IProjectManager
    {
        Task<AttendToProjectCommandResponse> AttendToProjectAsync(AttendToProjectCommandRequest request);
        Task<CreateProjectCommandResponse> CreateProjectAsync(CreateProjectCommandRequest request);
    }
}
