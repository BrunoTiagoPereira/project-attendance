using FluentAssertions;
using Moq;
using ProjectAttendance.Host.Application.Projects.Commands.Requests;
using ProjectAttendance.Host.Application.Projects.Services;
using ProjectAttendance.Host.Controllers;
using Xunit;

namespace ProjectAttendance.Host.Tests.Controllers;

public class ProjectsControllerTests
{
    private ProjectsController _testClass;
    private Mock<IProjectManager> _projectManager;

    public ProjectsControllerTests()
    {
        _projectManager = new Mock<IProjectManager>();
        _testClass = new ProjectsController(_projectManager.Object);
    }

    [Fact]
    public void CanConstruct()
    {
        // When
        var instance = new ProjectsController(_projectManager.Object);

        // Then
        instance.Should().NotBeNull();
    }

    [Fact]
    public void CannotConstructWithNullProjectManager()
    {
        FluentActions.Invoking(() => new ProjectsController(default(IProjectManager))).Should().Throw<ArgumentNullException>().WithParameterName("projectManager");
    }

    [Fact]
    public async Task CanCallAttendToProject()
    {
        // Given
        var request = new AttendToProjectCommandRequest { UserId = 75710797L, ProjectId = 1825782255L, StartedAt = DateTime.UtcNow, EndedAt = DateTime.UtcNow };

        _projectManager.Setup(mock => mock.AttendToProjectAsync(It.IsAny<AttendToProjectCommandRequest>())).ReturnsAsync(new AttendToProjectCommandResponse { WorkTime = new AttendToProjectWorkTimeCommandResponse { WorkTimeId = 1983645963L, UserId = 1658643017L, ProjectId = 1731828042L, StartedAt = DateTime.UtcNow, EndedAt = DateTime.UtcNow } });

        // When
        var result = await _testClass.AttendToProject(request);

        // Then
        _projectManager.Verify(mock => mock.AttendToProjectAsync(It.IsAny<AttendToProjectCommandRequest>()));
    }

    [Fact]
    public async Task CanCallCreateProject()
    {
        // Given
        var request = new CreateProjectCommandRequest { Title = "project", Description = "project" };

        _projectManager.Setup(mock => mock.CreateProjectAsync(It.IsAny<CreateProjectCommandRequest>())).ReturnsAsync(new CreateProjectCommandResponse { Project = new CreateProjectProjectCommandResponse { ProjectId = 1, Title = "project", Description = "project", UsersIds = new List<long>() } });

        // When
        var result = await _testClass.CreateProject(request);

        // Then
        _projectManager.Verify(mock => mock.CreateProjectAsync(It.IsAny<CreateProjectCommandRequest>()));
    }
}