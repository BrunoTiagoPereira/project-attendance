using ProjectAttendance.Core.Data.Repositories;
using ProjectAttendance.Domain.Projects.Entities;

namespace ProjectAttendance.Domain.Projects.Repositories
{
    public interface IProjectRepository : IRepository<Project>
    {
        Task<Project?> GetProjectWithUsersAndWorkTimesAsync(long projectId);

        Task<Project[]> GetProjectstFromUserAsync(long userId);
    }
}
