using FluentAssertions;
using FluentValidation.TestHelper;
using ProjectAttendance.Host.Application.Users.Commands.Requests;
using ProjectAttendance.Host.Application.Users.Commands.Validators;
using Xunit;

namespace ProjectAttendance.Host.Tests.Application.Users.Commands.Validators;

public class CreateUserCommandRequestValidatorTests
{
    private CreateUserCommandRequestValidator _testClass;

    public CreateUserCommandRequestValidatorTests()
    {
        _testClass = new CreateUserCommandRequestValidator();
    }

    [Fact]
    public void CanConstruct()
    {
        // When
        var instance = new CreateUserCommandRequestValidator();

        // Then
        instance.Should().NotBeNull();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void ShouldValidateWhenInvalidEmail(string email)
    {
        // When
        var result = _testClass.TestValidate(new CreateUserCommandRequest { Email = email });

        // Then
        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void ShouldValidateWhenInvalidUsername(string password)
    {
        // When
        var result = _testClass.TestValidate(new CreateUserCommandRequest { Username = password });

        // Then
        result.ShouldHaveValidationErrorFor(x => x.Username);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void ShouldValidateWhenInvalidLogin(string login)
    {
        // When
        var result = _testClass.TestValidate(new CreateUserCommandRequest { Login = login });

        // Then
        result.ShouldHaveValidationErrorFor(x => x.Login);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void ShouldValidateWhenInvalidPassword(string password)
    {
        // When
        var result = _testClass.TestValidate(new CreateUserCommandRequest { Password = password });

        // Then
        result.ShouldHaveValidationErrorFor(x => x.Password);
    }

    [Fact]
    public void ShouldHaveNoErrors()
    {
        // When
        var result = _testClass.TestValidate(new CreateUserCommandRequest { Email = "admin@admin.com", Username = "admin", Login = "admin", Password = "admin123" });

        // Then
        result.ShouldNotHaveAnyValidationErrors();
    }
}