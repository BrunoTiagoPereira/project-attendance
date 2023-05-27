using Bogus;
using FluentAssertions;
using ProjectAttendance.Core.Exceptions;
using ProjectAttendance.Domain.Projects.Entities;
using ProjectAttendance.Domain.Users.Entities;
using ProjectAttendance.UnitTests.Core.Fakers;
using Xunit;

namespace ProjectAttendance.Domain.Tests.Projects.Entities
{
    public class ProjectTests
    {
        private readonly Faker _faker;
        private readonly ProjectFaker _projectFaker;
        private readonly UserFaker _userFaker;

        public ProjectTests()
        {
            _faker = new Faker();
            _projectFaker = new ProjectFaker();
            _userFaker = new UserFaker();
        }

        [Fact]
        public void CanConstruct()
        {
            // Given
            var title = _faker.Company.CompanyName();
            var description = _faker.Company.CatchPhrase();
            var user = _userFaker.Generate();

            // When
            var result = new Project(title, description, new[] { user });

            // Then
            result.Title.Should().Be(title);
            result.Description.Should().Be(description);
            result.Users.Should().NotBeEmpty();
            result.Users.First().Should().Be(user);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void ShouldThrowWhenUpdatingTitleAndItIsInvalid(string title)
        {
            // Given
            var project = _projectFaker.Generate();

            // When // Then
            FluentActions.Invoking(() => project.UpdateTitle(title)).Should().Throw<DomainException>();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void ShouldThrowWhenUpdatingDescriptionAndItIsInvalid(string description)
        {
            // Given
            var project = _projectFaker.Generate();

            // When // Then
            FluentActions.Invoking(() => project.UpdateDescription(description)).Should().Throw<DomainException>();
        }

        [Fact]
        public void ShouldThrowWhenUpdatingUsersAndItIsNull()
        {
            // Given
            var project = _projectFaker.Generate();

            // When // Then
            FluentActions.Invoking(() => project.UpdateUsers(default(IEnumerable<User>))).Should().Throw<DomainException>();
        }

        [Fact]
        public void ShouldThrowWhenUpdatingUsersAndItIsEmpty()
        {
            // Given
            var project = _projectFaker.Generate();

            // When // Then
            FluentActions.Invoking(() => project.UpdateUsers(Enumerable.Empty<User>())).Should().Throw<DomainException>();
        }

        [Fact]
        public void ShouldThrowWhenAttendingToAndUserIsNull()
        {
            // Given
            var project = _projectFaker.Generate();

            // When // Then
            FluentActions.Invoking(() => project.AttendTo(default(User), DateTime.Now.AddDays(-2), DateTime.Now.AddDays(-1))).Should().Throw<DomainException>();
        }

        [Fact]
        public void ShouldThrowWhenAttendingToAndUserIsNotInProject()
        {
            // Given
            var project = _projectFaker.Generate();
            var user = _userFaker.Generate();

            // When // Then
            FluentActions.Invoking(() => project.AttendTo(user, DateTime.Now.AddDays(-2), DateTime.Now.AddDays(-1))).Should().Throw<DomainException>();
        }

        [Fact]
        public void ShouldThrowWhenAttendingToAndStartedAtIsAfterNow()
        {
            // Given
            var project = _projectFaker.Generate();
            var projectUser = project.Users.First();

            // When // Then
            FluentActions.Invoking(() => project.AttendTo(projectUser, DateTime.Now.AddDays(1), DateTime.Now.AddDays(-1))).Should().Throw<DomainException>();
        }

        [Fact]
        public void ShouldThrowWhenAttendingToAndStartedAtIsGreaterThanEndedAt()
        {
            // Given
            var project = _projectFaker.Generate();
            var projectUser = project.Users.First();

            // When // Then
            FluentActions.Invoking(() => project.AttendTo(projectUser, DateTime.Now.AddDays(-1), DateTime.Now.AddDays(-2))).Should().Throw<DomainException>();
        }

        [Fact]
        public void ShouldThrowWhenAttendingToAndEndedAtIsAfterNow()
        {
            // Given
            var project = _projectFaker.Generate();
            var projectUser = project.Users.First();

            // When // Then
            FluentActions.Invoking(() => project.AttendTo(projectUser, DateTime.Now.AddDays(-2), DateTime.Now.AddDays(1))).Should().Throw<DomainException>();
        }

        [Fact]
        public void ShouldUpdateTitle()
        {
            // Given
            var project = _projectFaker.Generate();
            var title = _faker.Company.CompanyName();

            // When
            project.UpdateTitle(title);

            // Then
            project.Title.Should().Be(title);
        }

        [Fact]
        public void ShouldUpdateDescription()
        {
            // Given
            var project = _projectFaker.Generate();
            var description = _faker.Company.CatchPhrase();

            // When
            project.UpdateDescription(description);

            // Then
            project.Description.Should().Be(description);
        }

        [Fact]
        public void ShouldUpdateUsers()
        {
            // Given
            var project = _projectFaker.Generate();
            var user = _userFaker.Generate();

            // When
            project.UpdateUsers(new[] { user });

            // Then
            project.Users.Should().NotBeEmpty();
            project.Users.First().Should().Be(user);
        }

        [Fact]
        public void ShouldAttendTo()
        {
            // Given
            var project = _projectFaker.Generate();
            var projectUser = project.Users.First();
            var startedAt = DateTime.Now.AddDays(-2);
            var endedAt = DateTime.Now.AddDays(-1);

            // When
            var result = project.AttendTo(projectUser, startedAt, endedAt);

            // Then
            result.Project.Should().Be(project);
            result.User.Should().Be(projectUser);
            result.StartedAt.Should().Be(startedAt);
            result.EndedAt.Should().Be(endedAt);
            project.WorkTimes.Should().NotBeEmpty();
            project.WorkTimes.First().Should().Be(result);
        }
    }
}