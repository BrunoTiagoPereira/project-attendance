using FluentAssertions;
using FluentValidation.TestHelper;
using ProjectAttendance.Host.Application.Projects.Commands.Requests;
using ProjectAttendance.Host.Application.Users.Commands.Validators;
using Xunit;

namespace ProjectAttendance.Host.Tests.Application.Users.Commands.Validators;

public class UpdateProjectCommandRequestValidatorTests
{
    private UpdateProjectCommandRequestValidator _testClass;

    public UpdateProjectCommandRequestValidatorTests()
    {
        _testClass = new UpdateProjectCommandRequestValidator();
    }

    [Fact]
    public void CanConstruct()
    {
        // When
        var instance = new UpdateProjectCommandRequestValidator();

        // Then
        instance.Should().NotBeNull();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void ShouldValidateWhenInvalidTitle(string title)
    {
        // When
        var result = _testClass.TestValidate(new UpdateProjectCommandRequest { Title = title });

        // Then
        result.ShouldHaveValidationErrorFor(x => x.Title);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void ShouldValidateWhenInvalidDescription(string description)
    {
        // When
        var result = _testClass.TestValidate(new UpdateProjectCommandRequest { Description = description });

        // Then
        result.ShouldHaveValidationErrorFor(x => x.Description);
    }

    [Fact]
    public void ShouldHaveNoErrors()
    {
        // When
        var result = _testClass.TestValidate(new UpdateProjectCommandRequest { Title = "project", Description = "project" });

        // Then
        result.ShouldNotHaveAnyValidationErrors();
    }
}