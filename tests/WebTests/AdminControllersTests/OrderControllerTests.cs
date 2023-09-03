using AutoMapper;
using GameRental.Application.DTO.Order;
using GameRental.Application.Interfaces;
using GameRental.Web.Areas.Administration.Controllers;
using GameRental.Web.Models;
using GameRental.Web.Models.Order;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace WebTets.AdminControllersTests;

public class OrderControllerTests
{
    private Mock<IMapper> _mapper;
    private Mock<IServiceManager> _serviceManagerMoq;
    private OrderController _controller;

    [SetUp]
    public void SetUp()
    {
        _mapper = new Mock<IMapper>();
        _serviceManagerMoq = new Mock<IServiceManager>();
        _controller = new OrderController(_mapper.Object, _serviceManagerMoq.Object);
        _serviceManagerMoq.Setup(u => u.OrderService).Returns(new Mock<IOrderService>().Object);
    }
    
    [Test]
    public async Task Index_ReturnViewResultWithOrders()
    {
        var ordersDto = new List<OrderDto>() { new OrderDto(), new OrderDto() };
        var ordersVm = new List<OrderVm>() {new OrderVm(),new OrderVm() };
        _mapper.Setup(u => u.Map<List<OrderVm>>(ordersDto)).Returns(ordersVm);
        _serviceManagerMoq.Setup(u => u.OrderService.GetOrdersAsync(null,
            o=>o.Account,
            o=>o.User)).ReturnsAsync(ordersDto);
        
        //act
        var result = await _controller.Index();
        
        //assert
        Assert.IsInstanceOf<ViewResult>(result);
        var viewResult = result as ViewResult;
        Assert.IsAssignableFrom<List<OrderVm>>(viewResult?.Model);

    }

}