using Microsoft.AspNetCore.Mvc;

namespace Event.Controllers.SuperAdmin
{
    public class SuperAdminController : Controller
    {
        public IActionResult Dashboard(string username)
        {
            ViewBag.Username = username;
            return View();
        }
    }
}
