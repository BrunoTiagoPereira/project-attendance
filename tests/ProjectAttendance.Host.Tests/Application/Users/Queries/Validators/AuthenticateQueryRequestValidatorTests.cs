using FluentAssertions;
using FluentValidation.TestHelper;
using ProjectAttendance.Host.Application.Users.Queries.Requests;
using Xunit;

namespace ProjectAttendance.Host.Tests.Application.Users.Queries.Requests;

public class AuthenticateQueryRequestValidatorTests
{
    private AuthenticateQueryRequestValidator _testClass;

    public AuthenticateQueryRequestValidatorTests()
    {
        _testClass = new AuthenticateQueryRequestValidator();
    }

    [Fact]
    public void CanConstruct()
    {
        // When
        var instance = new AuthenticateQueryRequestValidator();

        // Then
        instance.Should().NotBeNull();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void ShouldValidateWhenInvalidLogin(string login)
    {
        // When
        var result = _testClass.TestValidate(new AuthenticateQueryRequest { Login = login });

        // Then
        result.ShouldHaveValidationErrorFor(x => x.Login);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void ShouldValidateWhenInvalidPassword(string password)
    {
        // When
        var result = _testClass.TestValidate(new AuthenticateQueryRequest { Password = password });

        // Then
        result.ShouldHaveValidationErrorFor(x => x.Password);
    }

    [Fact]
    public void ShouldHaveNoErrors()
    {
        // When
        var result = _testClass.TestValidate(new AuthenticateQueryRequest { Login = "admin", Password = "admin123" });

        // Then
        result.ShouldNotHaveAnyValidationErrors();
    }
}