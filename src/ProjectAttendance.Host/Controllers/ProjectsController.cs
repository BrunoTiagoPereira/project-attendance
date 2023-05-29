using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectAttendance.Host.Application.Projects.Commands.Requests;
using ProjectAttendance.Host.Application.Projects.Queries.Requests;
using ProjectAttendance.Host.Application.Projects.Queries.Responses;
using ProjectAttendance.Host.Application.Projects.Services;

namespace ProjectAttendance.Host.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/v1")]
    public class ProjectsController : Controller
    {
        private readonly IProjectManager _projectManager;

        public ProjectsController(IProjectManager projectManager)
        {
            _projectManager = projectManager ?? throw new ArgumentNullException(nameof(projectManager));
        }

        [HttpPost]
        [Route("times")]
        public Task<AttendToProjectCommandResponse> AttendToProject(AttendToProjectCommandRequest request)
        {
            return _projectManager.AttendToProjectAsync(request);
        }

        [HttpPost]
        [Route("projects")]
        public Task<CreateProjectCommandResponse> CreateProject(CreateProjectCommandRequest request)
        {
            return _projectManager.CreateProjectAsync(request);
        }

        [HttpGet]
        [Route("projects/{projectId}")]
        public Task<GetProjectQueryResponse> GetProject(long projectId)
        {
            return _projectManager.GetProjectAsync(new GetProjectQueryRequest { ProjectId = projectId });
        }
    }
}