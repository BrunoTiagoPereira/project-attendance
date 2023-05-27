using Microsoft.EntityFrameworkCore;
using ProjectAttendance.Core.Data.Repositories;
using ProjectAttendance.Domain.Projects.Entities;
using ProjectAttendance.Domain.Projects.Repositories;

namespace ProjectAttendance.Infra.Repositories
{
    public class ProjectRepository : Repository<Project>, IProjectRepository
    {
        public ProjectRepository(DbContext context) : base(context)
        {
        }

        public Task<Project?> GetProjectWithUsersAndWorkTimesAsync(long projectId)
        {
            return Set
                .AsTracking()
                .Include(x => x.Users)
                .Include(x => x.WorkTimes)
                .FirstOrDefaultAsync(x => x.Id == projectId)
                ;
        }
    }
}