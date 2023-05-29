using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bogus;
using FluentAssertions;
using Moq;
using ProjectAttendance.Application.Users.Services;
using ProjectAttendance.Core.Exceptions;
using ProjectAttendance.Core.Transaction;
using ProjectAttendance.Core.Validators;
using ProjectAttendance.Domain.Projects.Entities;
using ProjectAttendance.Domain.Projects.Repositories;
using ProjectAttendance.Domain.Users.Entities;
using ProjectAttendance.Domain.Users.Repositories;
using ProjectAttendance.Host.Application.Projects.Commands.Requests;
using ProjectAttendance.Host.Application.Projects.Services;
using ProjectAttendance.Infra;
using ProjectAttendance.Infra.Repositories;
using ProjectAttendance.UnitTests.Core.Fakers;
using Xunit;

namespace ProjectAttendance.Host.Tests.Application.Projects.Services;

public class ProjectManagerTests
{
    private readonly ProjectManager _testClass;
    private readonly Mock<IValidatorManager> _validatorManager;
    private readonly Mock<IUserRepository> _userRepository;
    private readonly Mock<IProjectRepository> _projectRepository;
    private readonly Mock<IUserAccessorManager> _userAccessorManager;
    private readonly Mock<IUnitOfWork> _uow;
    private readonly UserFaker _userFaker;
    private readonly ProjectFaker _projectFaker;
    private readonly Faker _faker;
    private readonly DatabaseContext _databaseContext;

    public ProjectManagerTests()
    {
        _validatorManager = new Mock<IValidatorManager>();
        _userRepository = new Mock<IUserRepository>();
        _projectRepository = new Mock<IProjectRepository>();
        _userAccessorManager = new Mock<IUserAccessorManager>();
        _uow = new Mock<IUnitOfWork>();
        _testClass = new ProjectManager(_validatorManager.Object, _userRepository.Object, _projectRepository.Object, _userAccessorManager.Object, _uow.Object);
        _userFaker = new UserFaker();
        _projectFaker = new ProjectFaker();
        _faker = new Faker();
        _databaseContext = new DatabaseContextFaker().Generate();
    }

    [Fact]
    public void CanConstruct()
    {
        // When
        var instance = new ProjectManager(_validatorManager.Object, _userRepository.Object, _projectRepository.Object, _userAccessorManager.Object, _uow.Object);

        // Then
        instance.Should().NotBeNull();
    }

    [Fact]
    public void CannotConstructWithNullValidatorManager()
    {
        FluentActions.Invoking(() => new ProjectManager(default(IValidatorManager), new Mock<IUserRepository>().Object, new Mock<IProjectRepository>().Object, new Mock<IUserAccessorManager>().Object, new Mock<IUnitOfWork>().Object)).Should().Throw<ArgumentNullException>().WithParameterName("validatorManager");
    }

    [Fact]
    public void CannotConstructWithNullUserRepository()
    {
        FluentActions.Invoking(() => new ProjectManager(new Mock<IValidatorManager>().Object, default(IUserRepository), new Mock<IProjectRepository>().Object, new Mock<IUserAccessorManager>().Object, new Mock<IUnitOfWork>().Object)).Should().Throw<ArgumentNullException>().WithParameterName("userRepository");
    }

    [Fact]
    public void CannotConstructWithNullProjectRepository()
    {
        FluentActions.Invoking(() => new ProjectManager(new Mock<IValidatorManager>().Object, new Mock<IUserRepository>().Object, default(IProjectRepository), new Mock<IUserAccessorManager>().Object, new Mock<IUnitOfWork>().Object)).Should().Throw<ArgumentNullException>().WithParameterName("projectRepository");
    }

    [Fact]
    public void CannotConstructWithNullUserAccessorManager()
    {
        FluentActions.Invoking(() => new ProjectManager(new Mock<IValidatorManager>().Object, new Mock<IUserRepository>().Object, new Mock<IProjectRepository>().Object, default(IUserAccessorManager), new Mock<IUnitOfWork>().Object)).Should().Throw<ArgumentNullException>().WithParameterName("userAccessorManager");
    }

    [Fact]
    public void CannotConstructWithNullUow()
    {
        FluentActions.Invoking(() => new ProjectManager(new Mock<IValidatorManager>().Object, new Mock<IUserRepository>().Object, new Mock<IProjectRepository>().Object, new Mock<IUserAccessorManager>().Object, default(IUnitOfWork))).Should().Throw<ArgumentNullException>().WithParameterName("uow");
    }

    [Fact]
    public async Task ShouldThrowWhenCurrentUserIdIsNotTheRequestUser()
    {
        // Given
        _userAccessorManager.Setup(x => x.GetCurrentUserId()).Returns(1);

        // When // Then
        await FluentActions.Invoking(async () => await _testClass.AttendToProjectAsync(new AttendToProjectCommandRequest { UserId = 2 })).Should().ThrowAsync<DomainException>();
        _userAccessorManager.Verify(x => x.GetCurrentUserId());
        _uow.Verify(x => x.CommitAsync(), Times.Never());
    }

    [Fact]
    public async Task ShouldThrowWhenUserDoesNotExists()
    {
        // Given
        _userAccessorManager.Setup(x => x.GetCurrentUserId()).Returns(1);

        // When // Then
        await FluentActions.Invoking(async () => await _testClass.AttendToProjectAsync(new AttendToProjectCommandRequest { UserId = 1 })).Should().ThrowAsync<DomainException>();
        _userAccessorManager.Verify(x => x.GetCurrentUserId());
        _userRepository.Verify(x => x.FindAsync(1));
        _uow.Verify(x => x.CommitAsync(), Times.Never());
    }


    [Fact]
    public async Task ShouldThrowWhenProjectDoesNotExists()
    {
        // Given
        var user = _userFaker.Generate();
        _userAccessorManager.Setup(x => x.GetCurrentUserId()).Returns(1);
        _userRepository.Setup(x => x.FindAsync(1)).ReturnsAsync(user);

        // When // Then
        await FluentActions.Invoking(async () => await _testClass.AttendToProjectAsync(new AttendToProjectCommandRequest { UserId = 1 })).Should().ThrowAsync<DomainException>();
        _userAccessorManager.Verify(x => x.GetCurrentUserId());
        _userRepository.Verify(x => x.FindAsync(1));
        _uow.Verify(x => x.CommitAsync(), Times.Never());
    }

    [Fact]
    public async Task ShouldThrowWhenUserIsNotInProject()
    {
        // Given
        var manager = new ProjectManager(_validatorManager.Object, new UserRepository(_databaseContext), new ProjectRepository(_databaseContext), _userAccessorManager.Object, _uow.Object);
        var project = _projectFaker.Generate();
        var user = _userFaker.Generate();

        _databaseContext.AddRange(project, user);
        _databaseContext.SaveChanges();

        _userAccessorManager.Setup(x => x.GetCurrentUserId()).Returns(user.Id);

        // When // Then
        await FluentActions.Invoking(async () => await manager.AttendToProjectAsync(new AttendToProjectCommandRequest { UserId = user.Id, ProjectId = project.Id })).Should().ThrowAsync<DomainException>();
        _userAccessorManager.Verify(x => x.GetCurrentUserId());
        _uow.Verify(x => x.CommitAsync(), Times.Never());
    }

    [Fact]
    public async Task CanCallAttendToProject()
    {
        // Given
        var user = _userFaker.Generate();
        var project = _projectFaker.Generate();
        var startedAt = DateTime.Now.AddDays(-2);
        var endedAt = DateTime.Now.AddDays(-1);

        project.UpdateUsers(new[] { user });
        _userAccessorManager.Setup(x => x.GetCurrentUserId()).Returns(user.Id);
        _userRepository.Setup(x => x.FindAsync(user.Id)).ReturnsAsync(user);
        _projectRepository.Setup(x => x.GetProjectWithUsersAndWorkTimesAsync(project.Id)).ReturnsAsync(project);

        // When 
        var result = await _testClass.AttendToProjectAsync(new AttendToProjectCommandRequest { UserId = user.Id, ProjectId = project.Id, StartedAt = startedAt, EndedAt = endedAt });

        // Then
        _userAccessorManager.Verify(x => x.GetCurrentUserId());
        _userRepository.Verify(x => x.FindAsync(user.Id));
        _projectRepository.Verify(x => x.GetProjectWithUsersAndWorkTimesAsync(project.Id));
        _projectRepository.Verify(x => x.Update(project));
        _uow.Verify(x => x.CommitAsync());
        result.WorkTime.UserId.Should().Be(user.Id);
        result.WorkTime.ProjectId.Should().Be(project.Id);
        result.WorkTime.StartedAt.Should().Be(startedAt);
        result.WorkTime.EndedAt.Should().Be(endedAt);
    }

    [Fact]
    public async Task ShouldThrowWhenCreatingProjectAndAnyUserDoesNotExists()
    {
        // Given
        var manager = new ProjectManager(_validatorManager.Object, new UserRepository(_databaseContext), new ProjectRepository(_databaseContext), _userAccessorManager.Object, _uow.Object);
        var project = _projectFaker.Generate();
        var user = _userFaker.Generate();

        _databaseContext.SaveChanges();

        _userAccessorManager.Setup(x => x.GetCurrentUserId()).Returns(user.Id);

        // When // Then
        await FluentActions.Invoking(async () => await manager.CreateProjectAsync(new CreateProjectCommandRequest { Title = project.Title, Description = project.Description, UsersIds = new List<long> { user.Id, 5} })).Should().ThrowAsync<DomainException>();
        _userAccessorManager.Verify(x => x.GetCurrentUserId());
        _uow.Verify(x => x.CommitAsync(), Times.Never());
    }

    [Fact]
    public async Task CanCallCreateProject()
    {
        // Given
        var manager = new ProjectManager(_validatorManager.Object, new UserRepository(_databaseContext), new ProjectRepository(_databaseContext), _userAccessorManager.Object, _uow.Object);
        var user1 = _userFaker.Generate();
        var user2 = _userFaker.Generate();
        var project = _projectFaker.Generate();

        _databaseContext.AddRange(user1, user2, project);
        _databaseContext.SaveChanges();

        _userAccessorManager.Setup(x => x.GetCurrentUserId()).Returns(user1.Id);

        // When 
        var result = await manager.CreateProjectAsync(new CreateProjectCommandRequest { Title = project.Title, Description = project.Description, UsersIds = new List<long> { user2.Id } });

        // Then
        _uow.Verify(x => x.CommitAsync());
        _databaseContext.Set<Project>().Should().ContainSingle();
        result.Project.Title.Should().Be(project.Title);
        result.Project.Description.Should().Be(project.Description);
        _databaseContext.Set<Project>().Find(result.Project.ProjectId).Users.Should().HaveCount(2);
        _databaseContext.Set<Project>().Find(result.Project.ProjectId).Users.First().Should().Be(user1);
        _databaseContext.Set<Project>().Find(result.Project.ProjectId).Users.Last().Should().Be(user2);
    }
}