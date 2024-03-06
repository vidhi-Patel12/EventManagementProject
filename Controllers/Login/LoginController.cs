using Event.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data;
using Microsoft.AspNetCore.Session;
using Newtonsoft.Json.Linq;

namespace Event.Controllers.Login
{
    public class LoginController : Controller
    {

        
        public IActionResult Index()
        {        
            return View();
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(string login)
        {
            try
            {
                //HttpContext.Session.SetString("ID", 1);

                //Console.WriteLine(HttpContext.Session.SetString("userid"));
                string[]? logindata = JsonConvert.DeserializeObject<string[]>(login);
                //string roleid = EventAccessLayer.FetchUserProfileDetailsById(RegistrationID);
                var resultAdd = EventAccessLayer.Login(logindata);

                if (resultAdd.Rows.Count > 0)
                {
                    var DataList = new List<RegistrationModel>();
                    foreach (DataRow dr in resultAdd.Rows)
                    {
                        RegistrationModel Obj = new RegistrationModel();
                        Obj.RoleID = Convert.ToString(dr["RoleID"]);
                        Obj.Username = Convert.ToString(dr["UserName"]);
                        Obj.ID = Convert.ToInt32(dr["ID"]);
                        Obj.Name = Convert.ToString(dr["Name"]);
                        DataList.Add(Obj);
                    }
        

                    if (DataList != null)
                    {
                        var dataList = DataList.FirstOrDefault();
                        var Id = dataList.ID;
                        HttpContext.Session.SetInt32("Id", Id);
                        SessionClass.CreatedById = Id;


                        var UserName = dataList.Username;
                        HttpContext.Session.SetString("UserName", UserName);
                        SessionClass.UserName = UserName;

                        var Name = dataList.Name;
                        HttpContext.Session.SetString("Name", Name);
                        SessionClass.Name = Name;

                        if (dataList.RoleID == "Admin")
                        {
                            HttpContext.Session.SetInt32("UserId", Id);
                            HttpContext.Session.SetString("UserName", UserName);
                            return Json(new { IsSuccess = true, RedirectUrl = "/Admin/Dashboard", Username = logindata[0],dataList.RoleID,dataList.Name});

                        }
                        else if (dataList.RoleID == "Customer")
                        {
                            HttpContext.Session.SetInt32("UserId", Id);
                            return Json(new { IsSuccess = true, RedirectUrl = "/Customer/Dashboard", Username = logindata[0], dataList.RoleID,dataList.Name });
                        }
                        else if (dataList.RoleID == "SuperAdmin")
                        {
                            HttpContext.Session.SetInt32("UserId", Id);
                            return Json(new { IsSuccess = true, RedirectUrl = "/SuperAdmin/Dashboard", Username = logindata[0], dataList.RoleID,dataList.Name});
                        }
                        else
                        {
                            return Json(new { IsSuccess = false, Message = "Invalid role" });
                        }
                    }
                    else
                    {
                        return Json(new { IsSuccess = false, RedirectUrl = "/Registration/Create" });
                    }


                }
                return Json(new { IsSuccess = false, RedirectUrl = "/Registration/Create" });
            }

            catch
            {
                return Json(new { IsSuccess = false, Message = "An unexpected error occurred" });
            }

        }

    }
}
