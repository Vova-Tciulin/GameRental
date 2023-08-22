using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GameRental.Web.Areas.Administration.Controllers;

[Area("Administration")]
[Authorize(Roles = "Admin")]
public class HomeController:Controller
{
    public IActionResult Index()
    {
        return View();
    }
}