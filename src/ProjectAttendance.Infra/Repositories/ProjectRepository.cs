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
    }
}