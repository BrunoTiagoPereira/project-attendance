using FluentAssertions;
using FluentValidation.TestHelper;
using ProjectAttendance.Host.Application.Projects.Commands.Requests;
using ProjectAttendance.Host.Application.Users.Commands.Validators;
using Xunit;

namespace ProjectAttendance.Host.Tests.Application.Users.Commands.Validators;

public class CreateProjectCommandRequestValidatorTests
{
    private CreateProjectCommandRequestValidator _testClass;

    public CreateProjectCommandRequestValidatorTests()
    {
        _testClass = new CreateProjectCommandRequestValidator();
    }

    [Fact]
    public void CanConstruct()
    {
        // When
        var instance = new CreateProjectCommandRequestValidator();

        // Then
        instance.Should().NotBeNull();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void ShouldValidateWhenInvalidTitle(string title)
    {
        // When
        var result = _testClass.TestValidate(new CreateProjectCommandRequest { Title = title });

        // Then
        result.ShouldHaveValidationErrorFor(x => x.Title);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void ShouldValidateWhenInvalidDescription(string description)
    {
        // When
        var result = _testClass.TestValidate(new CreateProjectCommandRequest { Description = description });

        // Then
        result.ShouldHaveValidationErrorFor(x => x.Description);
    }

    [Fact]
    public void ShouldHaveNoErrors()
    {
        // When
        var result = _testClass.TestValidate(new CreateProjectCommandRequest { Title = "project", Description = "project" });

        // Then
        result.ShouldNotHaveAnyValidationErrors();
    }
}