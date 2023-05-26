using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Moq;
using ProjectAttendance.Core.Exceptions;
using ProjectAttendance.Core.Transaction;
using ProjectAttendance.Core.Validators;
using ProjectAttendance.Domain.Users.Entities;
using ProjectAttendance.Domain.Users.Repositories;
using ProjectAttendance.Host.Application.Users.Commands.Requests;
using ProjectAttendance.Host.Application.Users.Queries.Requests;
using ProjectAttendance.Host.Application.Users.Services;
using ProjectAttendance.UnitTests.Core.Fakers;
using Xunit;

namespace ProjectAttendance.Host.Tests.Application.Users.Services;

public class UserManagerTests
{
    private UserManager _testClass;
    private Mock<IValidatorManager> _validatorManager;
    private Mock<IUserRepository> _userRepository;
    private Mock<IUnitOfWork> _uow;
    private IConfiguration _configuration;
    private UserFaker _userFaker;

    public UserManagerTests()
    {
        _validatorManager = new Mock<IValidatorManager>();
        _userRepository = new Mock<IUserRepository>();
        _uow = new Mock<IUnitOfWork>();
        var configurationDictionary = new Dictionary<string, string>
        {
            { "JwtToken:Secret", Guid.NewGuid().ToString() }
        };

        _configuration = new ConfigurationBuilder().AddInMemoryCollection(configurationDictionary).Build();

        _testClass = new UserManager(_validatorManager.Object, _userRepository.Object, _uow.Object, _configuration);

        _userFaker = new UserFaker();
    }

    [Fact]
    public void CanConstruct()
    {
        // When
        var instance = new UserManager(_validatorManager.Object, _userRepository.Object, _uow.Object, _configuration);

        // Then
        instance.Should().NotBeNull();
    }

    [Fact]
    public void CannotConstructWithNullValidatorManager()
    {
        FluentActions.Invoking(() => new UserManager(default(IValidatorManager), new Mock<IUserRepository>().Object, new Mock<IUnitOfWork>().Object, new Mock<IConfiguration>().Object)).Should().Throw<ArgumentNullException>().WithParameterName("validatorManager");
    }

    [Fact]
    public void CannotConstructWithNullUserRepository()
    {
        FluentActions.Invoking(() => new UserManager(new Mock<IValidatorManager>().Object, default(IUserRepository), new Mock<IUnitOfWork>().Object, new Mock<IConfiguration>().Object)).Should().Throw<ArgumentNullException>().WithParameterName("userRepository");
    }

    [Fact]
    public void CannotConstructWithNullUow()
    {
        FluentActions.Invoking(() => new UserManager(new Mock<IValidatorManager>().Object, new Mock<IUserRepository>().Object, default(IUnitOfWork), new Mock<IConfiguration>().Object)).Should().Throw<ArgumentNullException>().WithParameterName("uow");
    }

    [Fact]
    public void CannotConstructWithNullConfiguration()
    {
        FluentActions.Invoking(() => new UserManager(new Mock<IValidatorManager>().Object, new Mock<IUserRepository>().Object, new Mock<IUnitOfWork>().Object, default(IConfiguration))).Should().Throw<ArgumentNullException>().WithParameterName("configuration");
    }

    [Fact]
    public async Task CanCallAuthenticate()
    {
        // Given
        var request = new AuthenticateQueryRequest { Login = "TestValue1003334164", Password = "TestValue1608749491" };
        var user = new User("TestValue1341857182", "TestValue421070978", "email@email.com.br", "TestValue430616039");

        _validatorManager.Setup(mock => mock.ThrowIfInvalid<AuthenticateQueryRequest>(It.IsAny<AuthenticateQueryRequest>())).Verifiable();
        _userRepository.Setup(mock => mock.FindByLoginAsync(It.IsAny<string>())).ReturnsAsync(user);

        // When
        var result = await _testClass.Authenticate(request);

        // Then
        _validatorManager.Verify(mock => mock.ThrowIfInvalid<AuthenticateQueryRequest>(It.IsAny<AuthenticateQueryRequest>()));
        _userRepository.Verify(mock => mock.FindByLoginAsync(It.IsAny<string>()));
        result.User.Email.Should().Be(user.Email.Value);
        result.User.Username.Should().Be(user.Username);
        result.User.Login.Should().Be(user.Login);
        result.User.Password.Should().Be(user.Password.Hash);
    }

    [Fact]
    public async Task CanCallCreateUser()
    {
        // Given
        var request = new CreateUserCommandRequest { Username = "TestValue653007980", Email = "email@email.com.br", Login = "TestValue245364778", Password = "TestValue1372279995" };

        _validatorManager.Setup(mock => mock.ThrowIfInvalid<CreateUserCommandRequest>(It.IsAny<CreateUserCommandRequest>())).Verifiable();

        // When
        var result = await _testClass.CreateUser(request);

        // Then
        _validatorManager.Verify(mock => mock.ThrowIfInvalid<CreateUserCommandRequest>(It.IsAny<CreateUserCommandRequest>()));
        _uow.Verify(mock => mock.CommitAsync());
        result.Username.Should().Be(request.Username);
        result.Email.Should().Be(request.Email);
        result.Login.Should().Be(request.Login);
    }

    [Fact]
    public async Task ShowThrowWhenTryToGetUserAndItDoesNotExists()
    {
        // Given
        var request = new GetUserQueryRequest { UserId = 1 };
        var user = _userFaker.Generate();

        _validatorManager.Setup(mock => mock.ThrowIfInvalid<GetUserQueryRequest>(It.IsAny<GetUserQueryRequest>())).Verifiable();

        // When  // Then
        await FluentActions.Invoking(async () => await _testClass.GetUser(request)).Should().ThrowAsync<DomainException>();
    }

    [Fact]
    public async Task CanCallGetUser()
    {
        // Given
        var request = new GetUserQueryRequest { UserId = 1 };
        var user = _userFaker.Generate();

        _validatorManager.Setup(mock => mock.ThrowIfInvalid<GetUserQueryRequest>(It.IsAny<GetUserQueryRequest>())).Verifiable();
        _userRepository.Setup(x => x.FindAsync(1)).ReturnsAsync(user);

        // When
        var result = await _testClass.GetUser(request);

        // Then
        _validatorManager.Verify(mock => mock.ThrowIfInvalid<GetUserQueryRequest>(It.IsAny<GetUserQueryRequest>()));
        result.User.Email.Should().Be(user.Email.Value);
        result.User.Username.Should().Be(user.Username);
        result.User.Login.Should().Be(user.Login);
        result.User.Id.Should().Be(user.Id);
    }

    //[Fact]
    //public async Task CanCallUpdateUser()
    //{
    //    // Given
    //    var request = new UpdateUserCommandRequest();

    //    // When
    //    var result = await _testClass.UpdateUser(request);

    //    // Then
    //}
}