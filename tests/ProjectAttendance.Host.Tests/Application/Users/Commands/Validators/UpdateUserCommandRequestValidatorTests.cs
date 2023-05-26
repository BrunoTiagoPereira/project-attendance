using FluentAssertions;
using FluentValidation.TestHelper;
using ProjectAttendance.Host.Application.Users.Commands.Requests;
using ProjectAttendance.Host.Application.Users.Commands.Validators;
using Xunit;

namespace ProjectAttendance.Host.Tests.Application.Users.Commands.Validators;

public class UpdateUserCommandRequestValidatorTests
{
    private UpdateUserCommandRequestValidator _testClass;

    public UpdateUserCommandRequestValidatorTests()
    {
        _testClass = new UpdateUserCommandRequestValidator();
    }

    [Fact]
    public void CanConstruct()
    {
        // When
        var instance = new UpdateUserCommandRequestValidator();

        // Then
        instance.Should().NotBeNull();
    }
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void ShouldValidateWhenInvalidUserId(long userId)
    {
        // When
        var result = _testClass.TestValidate(new UpdateUserCommandRequest { UserId = userId });

        // Then
        result.ShouldHaveValidationErrorFor(x => x.UserId);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void ShouldValidateWhenInvalidEmail(string email)
    {
        // When
        var result = _testClass.TestValidate(new UpdateUserCommandRequest { Email = email });

        // Then
        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void ShouldValidateWhenInvalidUsername(string password)
    {
        // When
        var result = _testClass.TestValidate(new UpdateUserCommandRequest { Username = password });

        // Then
        result.ShouldHaveValidationErrorFor(x => x.Username);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void ShouldValidateWhenInvalidLogin(string login)
    {
        // When
        var result = _testClass.TestValidate(new UpdateUserCommandRequest { Login = login });

        // Then
        result.ShouldHaveValidationErrorFor(x => x.Login);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void ShouldValidateWhenInvalidPassword(string password)
    {
        // When
        var result = _testClass.TestValidate(new UpdateUserCommandRequest { Password = password });

        // Then
        result.ShouldHaveValidationErrorFor(x => x.Password);
    }

    [Fact]
    public void ShouldHaveNoErrors()
    {
        // When
        var result = _testClass.TestValidate(new UpdateUserCommandRequest { UserId = 1, Email = "admin@admin.com", Username = "admin", Login = "admin", Password = "admin123" });

        // Then
        result.ShouldNotHaveAnyValidationErrors();
    }
}