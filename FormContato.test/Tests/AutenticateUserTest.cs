using FluentAssertions;
using FormContato.DTOs;
using FormContato.Models;
using FormContato.Repositories;
using FormContato.Services;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using Moq;
using System.Linq.Expressions;

namespace FormContato.test.Tests;
public class AutenticateUserTest
{
    private Mock<IUnitOfWork> _uof;
    private Mock<IPasswordHasher> _hasher;
    private Mock<AuthenticationService> _authService;
    private AuthenticateUserService _service;
    public UserModel user;
    public LoginDTO loginDTO;

    public AutenticateUserTest()
    {
        _uof = new Mock<IUnitOfWork>();
        _hasher = new Mock<IPasswordHasher>();
        _authService = new Mock<AuthenticationService>();
        _service = new AuthenticateUserService(_uof.Object, _hasher.Object, _authService.Object);
        user = new UserModel
        {
            Id = Guid.NewGuid(),
            Name = "Test",
            LastName = "User",
            Email = "test@example.com",
            Password = "password",
            Salt = "randomsalt",
            IsActive = true,
            CreatedAt = DateTime.Now,
            LastUpdatedAt = DateTime.Now,
            LastUpdatedBy = "TestUser",

        };

        loginDTO = new LoginDTO
        {
            Email = "test@example.com",
            Password = "password"
        };
    }

    [Fact]
    public async Task Authenticate_ShouldReturnError_UserNotFound()
    {
        var loginDTO = new LoginDTO
        {
            Email = "test@example.com",
            Password = "password"
        };
        _uof.Setup(x => x.UserRepository.Get(It.IsAny<Expression<Func<UserModel, bool>>>())).ReturnsAsync((UserModel)null);

        var result = await _service.Authenticate(loginDTO);
        result.Success.Should().BeFalse();
        result.Error.Should().Be("Invalid login attempt.");
    }

    //invalid password - 
    [Fact]
    public async Task Authenticate_ShouldReturnError_InvalidPassword()
    {
        user.Password = "wrongPassword";
        _uof.Setup(x => x.UserRepository.Get(It.IsAny<Expression<Func<UserModel, bool>>>())).ReturnsAsync(user);
        _hasher.Setup(x => x.IsValidPassword(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(false);

        var result = await _service.Authenticate(loginDTO);
        result.Success.Should().BeFalse();
        result.Error.Should().Be("Login or password are invalid.");
    }

    // user not active
    [Fact]
    public async Task Authenticate_ShouldReturnError_UserNotActive()
    {
        user.IsActive = false;
        _uof.Setup(x => x.UserRepository.Get(It.IsAny<Expression<Func<UserModel, bool>>>())).ReturnsAsync(user);
        _hasher.Setup(x => x.IsValidPassword(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(true);

        var result = await _service.Authenticate(loginDTO);
        result.Success.Should().BeFalse();
        result.Error.Should().Be("Invalid login attempt.");
    }

    // success
    [Fact]
    public async Task Authenticate_ShouldReturnSuccess()
    {
        _uof.Setup(x => x.UserRepository.Get(It.IsAny<Expression<Func<UserModel, bool>>>())).ReturnsAsync(user);
        _hasher.Setup(x => x.IsValidPassword(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(true);

        var result = await _service.Authenticate(loginDTO);
        result.Success.Should().BeTrue();
    }

    // prepareforauthentication
    // success
    // user not found

    // fetchuser
    // success
    // fail - null

    // isvalidpassword
    // true
    // false

}
