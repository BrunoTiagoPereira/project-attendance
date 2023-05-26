using FluentAssertions;
using Moq;
using ProjectAttendance.Host.Application.Users.Commands.Requests;
using ProjectAttendance.Host.Application.Users.Commands.Responses;
using ProjectAttendance.Host.Application.Users.Queries.Requests;
using ProjectAttendance.Host.Application.Users.Queries.Responses;
using ProjectAttendance.Host.Application.Users.Services;
using ProjectAttendance.Host.Controllers;
using Xunit;

namespace ProjectAttendance.Host.Tests.Controllers;

public class UsersControllerTests
{
    private readonly UsersController _testClass;
    private readonly Mock<IUserManager> _userManager;

    public UsersControllerTests()
    {
        _userManager = new Mock<IUserManager>();
        _testClass = new UsersController(_userManager.Object);
    }

    [Fact]
    public void CanConstruct()
    {
        // When
        var instance = new UsersController(_userManager.Object);

        // Then
        instance.Should().NotBeNull();
    }

    [Fact]
    public void CannotConstructWithNullUserManager()
    {
        FluentActions.Invoking(() => new UsersController(default(IUserManager))).Should().Throw<ArgumentNullException>().WithParameterName("userManager");
    }

    [Fact]
    public async Task CanCallAuthenticate()
    {
        // Given
        var request = new AuthenticateQueryRequest { Login = "admin", Password = "admin123" };
        var response = new AuthenticateQueryResponse { User = new AuthenticationUserResponse { Login = "admin", Password = "AUDH1239480ASKDJO123" }, Token = "123697eyo1u23uyohdaudsohuoh" };

        _userManager.Setup(mock => mock.Authenticate(It.IsAny<AuthenticateQueryRequest>())).ReturnsAsync(response);

        // When
        var result = await _testClass.Authenticate(request);

        // Then
        _userManager.Verify(mock => mock.Authenticate(It.IsAny<AuthenticateQueryRequest>()));
        result.Should().Be(response);
    }

    [Fact]
    public async Task CanCallCreateUser()
    {
        // Given
        var request = new CreateUserCommandRequest { Username = "admin", Email = "admin@admin.com.br", Login = "admin", Password = "AUDH1239480ASKDJO123" };
        var response = new CreateUserCommandResponse { Id = 1,  Login = "admin" };

        _userManager.Setup(mock => mock.CreateUser(It.IsAny<CreateUserCommandRequest>())).ReturnsAsync(response);

        // When
        var result = await _testClass.CreateUser(request);

        // Then
        _userManager.Verify(mock => mock.CreateUser(It.IsAny<CreateUserCommandRequest>()));
        result.Should().Be(response);
    }

    [Fact]
    public async Task CanCallGetUser()
    {
        // Given
        var request = new GetUserQueryRequest { UserId = 1 };
        var response = new GetUserQueryResponse { User = new GetUserQueryUserResponse { Id = 1 }   };

        _userManager.Setup(mock => mock.GetUser(It.IsAny<GetUserQueryRequest>())).ReturnsAsync(response);

        // When
        var result = await _testClass.GetUser(1);

        // Then
        _userManager.Verify(mock => mock.GetUser(It.Is<GetUserQueryRequest>(y => y.UserId == 1)));
        result.Should().Be(response);
    }
}