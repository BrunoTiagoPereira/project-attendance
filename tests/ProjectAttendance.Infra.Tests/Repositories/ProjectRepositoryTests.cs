using FluentAssertions;
using ProjectAttendance.Infra.Repositories;
using ProjectAttendance.UnitTests.Core.Fakers;
using Xunit;

namespace ProjectAttendance.Infra.Tests.Repositories;

public class ProjectRepositoryTests
{
    private readonly ProjectRepository _testClass;
    private readonly DatabaseContext _context;
    private readonly ProjectFaker _projectFaker;
    private readonly UserFaker _userFaker;

    public ProjectRepositoryTests()
    {
        _context = new DatabaseContextFaker().Generate();
        _testClass = new ProjectRepository(_context);
        _projectFaker = new ProjectFaker();
        _userFaker = new UserFaker();
    }

    [Fact]
    public void CanConstruct()
    {
        // When
        var instance = new ProjectRepository(_context);

        // Then
        instance.Should().NotBeNull();
    }

    [Fact]
    public void CannotConstructWithNullContext()
    {
        FluentActions.Invoking(() => new ProjectRepository(default(DatabaseContext))).Should().Throw<ArgumentNullException>().WithParameterName("context");
    }

    [Fact]
    public async Task CanCallGetProjectWithUsersAndWorkTimesAsync()
    {
        // Given
        var project = _projectFaker.Generate();
        var user = _userFaker.Generate();

        project.UpdateUsers(new[] { user });
        project.AttendTo(user, DateTime.Now.AddDays(-2), DateTime.Now.AddDays(-1));

        _context.AddRange(project, user);
        _context.SaveChanges();

        // When
        var result = await _testClass.GetProjectWithUsersAndWorkTimesAsync(project.Id);

        // Then
        result.Should().NotBeNull();
        result.Should().Be(project);
        result.Users.Should().NotBeEmpty();
        result.WorkTimes.Should().NotBeEmpty();
    }
}