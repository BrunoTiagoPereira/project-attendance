using FluentAssertions;
using FluentValidation.TestHelper;
using ProjectAttendance.Host.Application.Projects.Queries.Requests;
using ProjectAttendance.Host.Application.Projects.Queries.Validators;
using Xunit;

namespace ProjectAttendance.Host.Tests.Application.Projects.Queries.Validators;

public class GetProjectQueryRequestValidatorTests
{
    private GetProjectQueryRequestValidator _testClass;

    public GetProjectQueryRequestValidatorTests()
    {
        _testClass = new GetProjectQueryRequestValidator();
    }

    [Fact]
    public void CanConstruct()
    {
        // When
        var instance = new GetProjectQueryRequestValidator();

        // Then
        instance.Should().NotBeNull();
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(0)]
    public void ShouldValidateWhenInvalidProjectId(long projectId)
    {
        // When
        var result = _testClass.TestValidate(new GetProjectQueryRequest { ProjectId = projectId });

        // Then
        result.ShouldHaveValidationErrorFor(x => x.ProjectId);
    }

    [Fact]
    public void ShouldHaveNoErrors()
    {
        // When
        var result = _testClass.TestValidate(new GetProjectQueryRequest { ProjectId = 1 });

        // Then
        result.ShouldNotHaveAnyValidationErrors();
    }
}