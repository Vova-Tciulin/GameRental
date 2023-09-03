using GameRental.Web.Areas.Administration.Controllers;
using GameRental.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace WebTets.AdminControllersTests;

[TestFixture]
public class HomeControllerTests
{
    [Test]
    public void Index_ReturnView()
    {
        var controller = new HomeController();
        
        //act
        var result = controller.Index();
        
        //assert
        Assert.IsInstanceOf<ViewResult>(result);
    }
}