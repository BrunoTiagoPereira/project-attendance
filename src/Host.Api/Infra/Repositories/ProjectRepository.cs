using Host.Api.Core.Data.Repositories;
using Host.Api.Domain.Projects.Entities;
using Host.Api.Domain.Projects.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Host.Api.Infra.Repositories
{
    public class ProjectRepository : Repository<Project>, IProjectRepository
    {
        public ProjectRepository(DbContext context) : base(context)
        {
        }
    }
}