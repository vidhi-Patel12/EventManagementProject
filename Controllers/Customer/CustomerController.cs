using Microsoft.AspNetCore.Mvc;

namespace Event.Controllers.Customer
{
    public class CustomerController : Controller
    {
        public IActionResult Dashboard(string username)
        {
            ViewBag.Username = username;
            var Name = HttpContext.Session.GetString("Name");
            ViewBag.Name = Name;
            return View();
        }
    }
}
