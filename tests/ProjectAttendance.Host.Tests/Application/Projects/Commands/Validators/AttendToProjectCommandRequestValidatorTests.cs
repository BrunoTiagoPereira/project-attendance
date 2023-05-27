using FluentAssertions;
using FluentValidation.TestHelper;
using ProjectAttendance.Host.Application.Projects.Commands.Requests;
using ProjectAttendance.Host.Application.Users.Commands.Validators;
using Xunit;

namespace ProjectAttendance.Host.Tests.Application.Users.Commands.Validators;

public class AttendToProjectCommandRequestValidatorTests
{
    private AttendToProjectCommandRequestValidator _testClass;

    public AttendToProjectCommandRequestValidatorTests()
    {
        _testClass = new AttendToProjectCommandRequestValidator();
    }

    [Fact]
    public void CanConstruct()
    {
        // When
        var instance = new AttendToProjectCommandRequestValidator();

        // Then
        instance.Should().NotBeNull();
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(0)]
    public void ShouldValidateWhenInvalidUserId(long userId)
    {
        // When
        var result = _testClass.TestValidate(new AttendToProjectCommandRequest { UserId = userId });

        // Then
        result.ShouldHaveValidationErrorFor(x => x.UserId);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(0)]
    public void ShouldValidateWhenInvalidProjectId(long projectId)
    {
        // When
        var result = _testClass.TestValidate(new AttendToProjectCommandRequest { ProjectId = projectId });

        // Then
        result.ShouldHaveValidationErrorFor(x => x.ProjectId);
    }

    [Fact]
    public void ShouldValidateWhenInvalidStartedAt()
    {
        // When
        var result = _testClass.TestValidate(new AttendToProjectCommandRequest { });

        // Then
        result.ShouldHaveValidationErrorFor(x => x.StartedAt);
    }

    [Fact]
    public void ShouldValidateWhenStartedAtIsAfterNow()
    {
        // When
        var result = _testClass.TestValidate(new AttendToProjectCommandRequest { StartedAt = DateTime.Now.AddDays(1) });

        // Then
        result.ShouldHaveValidationErrorFor(x => x.StartedAt);
    }

    [Fact]
    public void ShouldValidateWhenStartedAtIsAfterEndedAt()
    {
        // When
        var result = _testClass.TestValidate(new AttendToProjectCommandRequest { StartedAt = DateTime.Now.AddDays(-1), EndedAt = DateTime.Now.AddDays(-2) });

        // Then
        result.ShouldHaveValidationErrorFor(x => x.StartedAt);
    }

    [Fact]
    public void ShouldValidateWhenInvalidEndedAt()
    {
        // When
        var result = _testClass.TestValidate(new AttendToProjectCommandRequest { });

        // Then
        result.ShouldHaveValidationErrorFor(x => x.EndedAt);
    }

    [Fact]
    public void ShouldValidateWhenEndedAtIsAfterNow()
    {
        // When
        var result = _testClass.TestValidate(new AttendToProjectCommandRequest { EndedAt = DateTime.Now.AddDays(1) });

        // Then
        result.ShouldHaveValidationErrorFor(x => x.EndedAt);
    }
}