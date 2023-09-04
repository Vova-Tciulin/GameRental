using AutoMapper;
using GameRental.Application.DTO.User;
using GameRental.Application.Interfaces;
using GameRental.Web.Models;
using GameRental.Web.Models.User;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace WebTets.AdminControllersTests;

[TestFixture]
public class UserController
{
    private Mock<IMapper> _mapper;
    private Mock<IServiceManager> _serviceManagerMoq;
    private GameRental.Web.Areas.Administration.Controllers.UserController _controller;

    [SetUp]
    public void SetUp()
    {
        _mapper = new Mock<IMapper>();
        _serviceManagerMoq = new Mock<IServiceManager>();
        _controller = new GameRental.Web.Areas.Administration.Controllers.UserController(_mapper.Object, _serviceManagerMoq.Object);
        _serviceManagerMoq.Setup(u => u.IdentityService).Returns(new Mock<IIdentityService>().Object);
    }

    [Test]
    public async Task Index_ReturnsUsersVm()
    {
        var usersDto = new List<UserDto>() { new UserDto() };
        var usersVm = new List<UserVm>() { new UserVm() };
        _mapper.Setup(u => u.Map<List<UserVm>>(usersDto)).Returns(usersVm);
        _serviceManagerMoq.Setup(u => u.IdentityService.GetAllUsersAsync()).ReturnsAsync(usersDto);
        
        //act
        var result = await _controller.Index();
        
        //Assert
        Assert.IsInstanceOf<ViewResult>(result);
        var viewResult = result as ViewResult;
        Assert.IsAssignableFrom<List<UserVm>>(viewResult?.Model);
    }
    
    [Test]
    public async Task UserDetails_ReturnsUserDetailsVm()
    {
        var id = "1";
        var userDetailsDto = new UserDetailsDto() { Id = "1" };
        var userDetailsVm = new UserDetailsVm() { Id = "1" };
        _mapper.Setup(u => u.Map<UserDetailsVm>(userDetailsDto)).Returns(userDetailsVm);
        _serviceManagerMoq.Setup(u => u.IdentityService.GetUserDetailsAsync(id)).ReturnsAsync(userDetailsDto);
        
        //act
        var result = await _controller.UserDetails(id);
        
        //Assert
        Assert.IsInstanceOf<ViewResult>(result);
        var viewResult = result as ViewResult;
        Assert.IsAssignableFrom<UserDetailsVm>(viewResult?.Model);
    }

    [Test]
    public async Task ChangeRole_Get_ReturnsUserChangeRoleVm()
    {
        var id = "1";
        var userDetailsDto = new UserDetailsDto() { Id = "1" };
        var userVm = new UserChangeRoleVm() { Id = "1" };
        _mapper.Setup(u => u.Map<UserChangeRoleVm>(userDetailsDto)).Returns(userVm);
        var allRoles = new List<string>() { new string("admin"), new string("user") };
        _serviceManagerMoq.Setup(u => u.IdentityService.GetAllRolesAsync()).ReturnsAsync(allRoles);
        _serviceManagerMoq.Setup(u => u.IdentityService.GetUserDetailsAsync(id)).ReturnsAsync(userDetailsDto);
        
        //act
        var result = await _controller.ChangeRole(id);
        
        //assert
        Assert.IsInstanceOf<ViewResult>(result);
        var viewResult = result as ViewResult;
        Assert.IsAssignableFrom<UserChangeRoleVm>(viewResult?.Model);
    }

    [Test]
    public async Task ChangeRole_Post_ValidModel_RedirectsToIndex()
    {
        var user = new UserChangeRoleVm();
        var userDto = new UserChangeRoleDto();
        _mapper.Setup(u => u.Map<UserChangeRoleDto>(user)).Returns(userDto);
        
        //act
        var result = await _controller.ChangeRole(user);
        
        //assert
        Assert.IsInstanceOf<RedirectToActionResult>(result);
        var redirectToAction = result as RedirectToActionResult;
        Assert.That(redirectToAction?.ActionName, Is.EqualTo("Index"));
        _serviceManagerMoq.Verify(u=>u.IdentityService.ChangeRoleAsync(userDto), Times.Once);
    }
    
}