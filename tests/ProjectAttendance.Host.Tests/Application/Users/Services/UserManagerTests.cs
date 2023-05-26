using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Moq;
using ProjectAttendance.Application.Users.Services;
using ProjectAttendance.Core.Exceptions;
using ProjectAttendance.Core.Transaction;
using ProjectAttendance.Core.Validators;
using ProjectAttendance.Domain.Users.Entities;
using ProjectAttendance.Domain.Users.Repositories;
using ProjectAttendance.Host.Application.Users.Commands.Requests;
using ProjectAttendance.Host.Application.Users.Queries.Requests;
using ProjectAttendance.Host.Application.Users.Services;
using ProjectAttendance.Infra;
using ProjectAttendance.Infra.Repositories;
using ProjectAttendance.UnitTests.Core.Fakers;
using Xunit;

namespace ProjectAttendance.Host.Tests.Application.Users.Services;

public class UserManagerTests
{
    private UserManager _testClass;
    private Mock<IValidatorManager> _validatorManager;
    private Mock<IUserRepository> _userRepository;
    private Mock<IUnitOfWork> _uow;
    private Mock<IUserAccessorManager> _userAccessorManager;
    private IConfiguration _configuration;
    private UserFaker _userFaker;
    private DatabaseContext _context;

    public UserManagerTests()
    {
        _validatorManager = new Mock<IValidatorManager>();
        _userRepository = new Mock<IUserRepository>();
        _uow = new Mock<IUnitOfWork>();
        _userAccessorManager = new Mock<IUserAccessorManager>();
        var configurationDictionary = new Dictionary<string, string>
        {
            { "JwtToken:Secret", Guid.NewGuid().ToString() }
        };

        _configuration = new ConfigurationBuilder().AddInMemoryCollection(configurationDictionary).Build();

        _testClass = new UserManager(_validatorManager.Object, _userRepository.Object, _uow.Object, _userAccessorManager.Object, _configuration);

        _userFaker = new UserFaker();

        _context = new DatabaseContextFaker().Generate();
    }

    [Fact]
    public void CanConstruct()
    {
        // When
        var instance = new UserManager(_validatorManager.Object, _userRepository.Object, _uow.Object, _userAccessorManager.Object, _configuration);

        // Then
        instance.Should().NotBeNull();
    }

    [Fact]
    public void CannotConstructWithNullValidatorManager()
    {
        FluentActions.Invoking(() => new UserManager(default(IValidatorManager), new Mock<IUserRepository>().Object, new Mock<IUnitOfWork>().Object, new Mock<IUserAccessorManager>().Object, new Mock<IConfiguration>().Object)).Should().Throw<ArgumentNullException>().WithParameterName("validatorManager");
    }

    [Fact]
    public void CannotConstructWithNullUserRepository()
    {
        FluentActions.Invoking(() => new UserManager(new Mock<IValidatorManager>().Object, default(IUserRepository), new Mock<IUnitOfWork>().Object, new Mock<IUserAccessorManager>().Object, new Mock<IConfiguration>().Object)).Should().Throw<ArgumentNullException>().WithParameterName("userRepository");
    }

    [Fact]
    public void CannotConstructWithNullUow()
    {
        FluentActions.Invoking(() => new UserManager(new Mock<IValidatorManager>().Object, new Mock<IUserRepository>().Object, default(IUnitOfWork), new Mock<IUserAccessorManager>().Object, new Mock<IConfiguration>().Object)).Should().Throw<ArgumentNullException>().WithParameterName("uow");
    }

    [Fact]
    public void CannotConstructWithNullUserAccessorManager()
    {
        FluentActions.Invoking(() => new UserManager(new Mock<IValidatorManager>().Object, new Mock<IUserRepository>().Object, new Mock<IUnitOfWork>().Object, default(IUserAccessorManager), new Mock<IConfiguration>().Object)).Should().Throw<ArgumentNullException>().WithParameterName("userAccessorManager");
    }

    [Fact]
    public void CannotConstructWithNullConfiguration()
    {
        FluentActions.Invoking(() => new UserManager(new Mock<IValidatorManager>().Object, new Mock<IUserRepository>().Object, new Mock<IUnitOfWork>().Object, new Mock<IUserAccessorManager>().Object, default(IConfiguration))).Should().Throw<ArgumentNullException>().WithParameterName("configuration");
    }

    [Fact]
    public async Task ShowThrowWhenTryToAuthenticateAndUserDoesNotExists()
    {
        // Given
        var request = new AuthenticateQueryRequest { };

        // When  // Then
        await FluentActions.Invoking(async () => await _testClass.Authenticate(request)).Should().ThrowAsync<DomainException>();
    }

    [Fact]
    public async Task ShowThrowWhenTryToAuthenticateAndPasswordIsIncorrect()
    {
        // Given
        var user = _userFaker.Generate();
        var request = new AuthenticateQueryRequest { Login = user.Login, Password = user.Password + "1" };

        _userRepository.Setup(x => x.FindByLoginAsync(user.Login)).ReturnsAsync(user);

        // When  // Then
        await FluentActions.Invoking(async () => await _testClass.Authenticate(request)).Should().ThrowAsync<DomainException>();
    }

    [Fact]
    public async Task CanCallAuthenticate()
    {
        // Given
        var user = new User("admin", "admin", "admin@admin.com", "admin");
        var request = new AuthenticateQueryRequest { Login = user.Login, Password = "admin" };

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
    public async Task ShowThrowWhenTryToCreateUserAndEmailIsTaken()
    {
        // Given
        var user = _userFaker.Generate();
        var request = new CreateUserCommandRequest { Email = user.Email.Value, Login = user.Login };

        _validatorManager.Setup(mock => mock.ThrowIfInvalid<GetUserQueryRequest>(It.IsAny<GetUserQueryRequest>())).Verifiable();
        _userRepository.Setup(x => x.EmailIsTakenAsync(user.Email.Value)).ReturnsAsync(true);

        // When  // Then
        await FluentActions.Invoking(async () => await _testClass.CreateUserAsync(request)).Should().ThrowAsync<DomainException>();
        _userRepository.Verify(x => x.EmailIsTakenAsync(user.Email.Value));
        _userRepository.Verify(x => x.LoginIsTakenAsync(user.Login), Times.Never());
    }

    [Fact]
    public async Task ShowThrowWhenTryToCreateUserAndLoginIsTaken()
    {
        // Given
        var user = _userFaker.Generate();
        var request = new CreateUserCommandRequest { Email = user.Email.Value, Login = user.Login };

        _validatorManager.Setup(mock => mock.ThrowIfInvalid<GetUserQueryRequest>(It.IsAny<GetUserQueryRequest>())).Verifiable();
        _userRepository.Setup(x => x.EmailIsTakenAsync(user.Email.Value)).ReturnsAsync(false);
        _userRepository.Setup(x => x.LoginIsTakenAsync(user.Login)).ReturnsAsync(true);

        // When  // Then
        await FluentActions.Invoking(async () => await _testClass.CreateUserAsync(request)).Should().ThrowAsync<DomainException>();
        _userRepository.Verify(x => x.EmailIsTakenAsync(user.Email.Value));
        _userRepository.Verify(x => x.LoginIsTakenAsync(user.Login));
    }

    [Fact]
    public async Task CanCallCreateUser()
    {
        // Given
        var request = new CreateUserCommandRequest { Username = "TestValue653007980", Email = "email@email.com.br", Login = "TestValue245364778", Password = "TestValue1372279995" };

        _validatorManager.Setup(mock => mock.ThrowIfInvalid<CreateUserCommandRequest>(It.IsAny<CreateUserCommandRequest>())).Verifiable();

        // When
        var result = await _testClass.CreateUserAsync(request);

        // Then
        _validatorManager.Verify(mock => mock.ThrowIfInvalid<CreateUserCommandRequest>(It.IsAny<CreateUserCommandRequest>()));
        _uow.Verify(mock => mock.CommitAsync());
        result.User.Username.Should().Be(request.Username);
        result.User.Email.Should().Be(request.Email);
        result.User.Login.Should().Be(request.Login);
    }

    [Fact]
    public async Task ShowThrowWhenTryToUpdateUserAndItIsNotTheCurrentUser()
    {
        // Given
        var manager = new UserManager(_validatorManager.Object, new UserRepository(_context), _uow.Object, _userAccessorManager.Object, _configuration);

        var user = _userFaker.Generate();
        var databaseUser = _userFaker.Generate();

        _context.AddRange(user, databaseUser);
        _context.SaveChanges();

        var request = new UpdateUserCommandRequest { UserId = user.Id, Email = user.Email.Value, Login = user.Login };

        _userAccessorManager.Setup(x => x.GetCurrentUserId()).Returns(5);
        _validatorManager.Setup(mock => mock.ThrowIfInvalid<UpdateUserCommandRequest>(It.IsAny<UpdateUserCommandRequest>())).Verifiable();

        // When  // Then
        await FluentActions.Invoking(async () => await manager.UpdateUserAsync(request)).Should().ThrowAsync<DomainException>();
        _validatorManager.Verify(x => x.ThrowIfInvalid(request));
    }

    [Fact]
    public async Task ShowThrowWhenTryToUpdateUserAndItIsNotExists()
    {
        // Given
        var manager = new UserManager(_validatorManager.Object, new UserRepository(_context), _uow.Object, _userAccessorManager.Object, _configuration);

        var user = _userFaker.Generate();

        var request = new UpdateUserCommandRequest { UserId = user.Id, Email = user.Email.Value, Login = user.Login };

        _userAccessorManager.Setup(x => x.GetCurrentUserId()).Returns(user.Id);
        _validatorManager.Setup(mock => mock.ThrowIfInvalid<UpdateUserCommandRequest>(It.IsAny<UpdateUserCommandRequest>())).Verifiable();

        // When  // Then
        await FluentActions.Invoking(async () => await manager.UpdateUserAsync(request)).Should().ThrowAsync<DomainException>();
        _validatorManager.Verify(x => x.ThrowIfInvalid(request));
    }

    [Fact]
    public async Task ShowThrowWhenTryToUpdateUserAndEmailIsTaken()
    {
        // Given
        var manager = new UserManager(_validatorManager.Object, new UserRepository(_context), _uow.Object, _userAccessorManager.Object, _configuration);

        var user = _userFaker.Generate();
        var databaseUser = _userFaker.Generate();

        _context.AddRange(user, databaseUser);
        _context.SaveChanges();

        var request = new UpdateUserCommandRequest { UserId = user.Id, Email = databaseUser.Email.Value, Login = user.Login };

        _userAccessorManager.Setup(x => x.GetCurrentUserId()).Returns(user.Id);
        _validatorManager.Setup(mock => mock.ThrowIfInvalid<UpdateUserCommandRequest>(It.IsAny<UpdateUserCommandRequest>())).Verifiable();

        // When  // Then
        await FluentActions.Invoking(async () => await manager.UpdateUserAsync(request)).Should().ThrowAsync<DomainException>();
        _validatorManager.Verify(x => x.ThrowIfInvalid(request));
    }

    [Fact]
    public async Task ShowThrowWhenTryToUpdateUserAndLoginIsTaken()
    {
        // Given
        var manager = new UserManager(_validatorManager.Object, new UserRepository(_context), _uow.Object, _userAccessorManager.Object, _configuration);

        var user = _userFaker.Generate();
        var databaseUser = _userFaker.Generate();

        _context.AddRange(user, databaseUser);
        _context.SaveChanges();

        var request = new UpdateUserCommandRequest { UserId = user.Id, Email = user.Email.Value, Login = databaseUser.Login };

        _userAccessorManager.Setup(x => x.GetCurrentUserId()).Returns(user.Id);
        _validatorManager.Setup(mock => mock.ThrowIfInvalid<UpdateUserCommandRequest>(It.IsAny<UpdateUserCommandRequest>())).Verifiable();

        // When  // Thens
        await FluentActions.Invoking(async () => await manager.UpdateUserAsync(request)).Should().ThrowAsync<DomainException>();
        _validatorManager.Verify(x => x.ThrowIfInvalid(request));
    }

    [Fact]
    public async Task CanCallUpdateUser()
    {
        // Given
        var manager = new UserManager(_validatorManager.Object, new UserRepository(_context), _uow.Object, _userAccessorManager.Object, _configuration);

        var user = _userFaker.Generate();
        var newUserValues = _userFaker.Generate();

        _context.AddRange(user);
        _context.SaveChanges();

        var request = new UpdateUserCommandRequest { UserId = user.Id, Email = newUserValues.Email.Value, Login = newUserValues.Login, Password = newUserValues.Password.Hash, Username = newUserValues.Username };

        _validatorManager.Setup(mock => mock.ThrowIfInvalid<UpdateUserCommandRequest>(It.IsAny<UpdateUserCommandRequest>())).Verifiable();
        _userAccessorManager.Setup(x => x.GetCurrentUserId()).Returns(user.Id);

        // When
        var result = await manager.UpdateUserAsync(request);

        // Then
        _validatorManager.Verify(mock => mock.ThrowIfInvalid<UpdateUserCommandRequest>(It.IsAny<UpdateUserCommandRequest>()));
        _uow.Verify(mock => mock.CommitAsync());
        _userAccessorManager.Verify(x => x.GetCurrentUserId());
        result.User.Username.Should().Be(request.Username);
        result.User.Email.Should().Be(request.Email);
        result.User.Login.Should().Be(request.Login);
        result.User.Username.Should().Be(request.Username);
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