using Microsoft.AspNetCore.Mvc;

namespace Event.Controllers.Admin
{
    public class AdminController : Controller
    {
        public IActionResult Dashboard(string username)
        {
            ViewBag.UserName = username;
            var Name = HttpContext.Session.GetString("Name");
            ViewBag.Name = Name;
            return View();
        }


    }
}
