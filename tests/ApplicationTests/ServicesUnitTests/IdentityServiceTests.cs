using AutoMapper;
using GameRental.Application.DTO.User;
using GameRental.Application.Services;
using GameRental.Domain.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;

namespace ApplicationTests.ServicesUnitTests;

[TestFixture]
public class IdentityServiceTests
{
    private Mock<UserManager<User>> _userManagerMock;
    private Mock<SignInManager<User>> _singInManagerMock;
    private Mock<IMapper> _mapperMock;
    private Mock<RoleManager<IdentityRole>> _roleManager;
    private IdentityService _identityService;

    [SetUp]
    public void SetUp()
    {
        _userManagerMock = new Mock<UserManager<User>>(new Mock<IUserStore<User>>().Object,
            new Mock<IOptions<IdentityOptions>>().Object,
            new Mock<IPasswordHasher<User>>().Object,
            new IUserValidator<User>[0],
            new IPasswordValidator<User>[0],
            new Mock<ILookupNormalizer>().Object,
            new Mock<IdentityErrorDescriber>().Object,
            new Mock<IServiceProvider>().Object,
            new Mock<ILogger<UserManager<User>>>().Object);
        _singInManagerMock = new Mock<SignInManager<User>>(_userManagerMock.Object,
            new Mock<IHttpContextAccessor>().Object,
            new Mock<IUserClaimsPrincipalFactory<User>>().Object,
            new Mock<IOptions<IdentityOptions>>().Object,
            new Mock<ILogger<SignInManager<User>>>().Object,
            new Mock<IAuthenticationSchemeProvider>().Object);
        _roleManager = new Mock<RoleManager<IdentityRole>>(
            new Mock<IRoleStore<IdentityRole>>().Object,
            new IRoleValidator<IdentityRole>[0],
            new Mock<ILookupNormalizer>().Object,
            new Mock<IdentityErrorDescriber>().Object,
            new Mock<ILogger<RoleManager<IdentityRole>>>().Object);
        _mapperMock = new Mock<IMapper>();
        
        _identityService = new IdentityService(_mapperMock.Object, _userManagerMock.Object, _singInManagerMock.Object,
            _roleManager.Object);
    }

}