using FluentAssertions;
using FluentValidation.TestHelper;
using ProjectAttendance.Host.Application.Users.Queries.Requests;
using Xunit;

namespace ProjectAttendance.Host.Tests.Application.Users.Queries.Requests;

public class GetUserQueryRequestValidatorTests
{
    private readonly GetUserQueryRequestValidator _testClass;

    public GetUserQueryRequestValidatorTests()
    {
        _testClass = new GetUserQueryRequestValidator();
    }

    [Fact]
    public void CanConstruct()
    {
        // When
        var instance = new GetUserQueryRequestValidator();

        // Then
        instance.Should().NotBeNull();
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(0)]
    public void ShouldValidateWhenInvalidUserId(long userId)
    {
        // When
        var result = _testClass.TestValidate(new GetUserQueryRequest { UserId = userId });

        // Then
        result.ShouldHaveValidationErrorFor(x => x.UserId);
    }

    [Fact]
    public void ShouldHaveNoErrors()
    {
        // When
        var result = _testClass.TestValidate(new GetUserQueryRequest { UserId = 1 });

        // Then
        result.ShouldNotHaveAnyValidationErrors();
    }
}