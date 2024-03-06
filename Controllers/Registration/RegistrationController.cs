using Event.Models;
using Event.Models.AccessLayer.AdminAccessLayer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.Elfie.Serialization;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System.Data;

namespace Event.Controllers.Registration
{
    public class RegistrationController : Controller
    {
        public IActionResult Index(string name)
        {
            ViewBag.Username = name;
            var Name = HttpContext.Session.GetString("Name");
            ViewBag.Name = Name;
            return View();
        }

        [HttpPost] 

      public JsonResult GetCountryList()
      {
           
            DataTable dt = EventAccessLayer.GetAllCountry();

            List<SelectListItem> list = new List<SelectListItem>();

            foreach (DataRow dr in dt.Rows)
            {
                list.Add(new SelectListItem
                {
                    Text = dr["Name"].ToString(),
                    Value = dr["CountryID"].ToString()
                });
            }
            return Json(new { list });
            
      }

        public JsonResult GetStateList(string CountryID)
        {
            DataTable dt = EventAccessLayer.GetState(CountryID);

            List<SelectListItem> statelist = new List<SelectListItem>();

            foreach (DataRow dr in dt.Rows)
            {
                statelist.Add(new SelectListItem
                {
                    Text = dr["StateName"].ToString(),
                    Value = dr["StateID"].ToString(),
                   // Value = dr["CountryID"].ToString()
                });
            }

            return Json(new { statelist });
        }


        public JsonResult GetCityList(string StateID)
        {
            DataTable dt = EventAccessLayer.GetCity(StateID);

            List<SelectListItem> citylist = new List<SelectListItem>();

            foreach (DataRow dr in dt.Rows)
            {
                citylist.Add(new SelectListItem
                {
                    Text = dr["CityName"].ToString(),
                    Value = dr["CityID"].ToString()
                });
            }

            return Json(new { citylist });
        }


        public IActionResult Create()
        {
            return View();
        }
        public ActionResult Register()
        {

            return View();
        }

        // POST: CustomerController/Create
        [HttpPost]
        public ActionResult Register(string Register)
        {
            string[]? register = JsonConvert.DeserializeObject<string[]>(Register);
            var resultAdd = EventAccessLayer.Registration(register);
            if (resultAdd)
            {
                return Json(new { IsSuccess = true, Message = "Register  added sucessfully", Name = Register[0] });
            }
            else
            {
                return Json(new { IsSuccess = false, Message = "Something went wrong while adding Customer details" });
            }
        }

        public IActionResult Privacy()
        {
            return View();

        }


        public ActionResult LoadUserProfile()
        {
            var result = EventAccessLayer.fetchUserProfile();
            if (result.Rows.Count > 0)
            {
                var DataList = new List<RegistrationModel>();
                foreach (DataRow dr in result.Rows)
                {
                    RegistrationModel Obj = new RegistrationModel();
                   
                    Obj.Username = Convert.ToString(dr["Username"]);
                    Obj.Mobileno = Convert.ToString(dr["Mobileno"]);
                    Obj.EmailID = Convert.ToString(dr["EmailID"]);
                    Obj.Gender = Convert.ToString(dr["Gender"]);
                    Obj.Birthdate = Convert.ToDateTime(dr["Birthdate"]);
                    Obj.Country = Convert.ToString(dr["Country"]);
                    Obj.State = Convert.ToString(dr["State"]);
                    Obj.City = Convert.ToString(dr["City"]);
                    Obj.Address = Convert.ToString(dr["Address"]);
                    Obj.CreatedOn = Convert.ToDateTime(dr["CreatedOn"]);
                    DataList.Add(Obj);
                }
                return Json(new { IsSuccess = true, Message = "", DataList });
            }
            else
            {
                return Json(new { IsSuccess = false, Message = "No records are available for this master" });
            }
        }


        //[HttpGet]
        public async Task<ActionResult> LoadUserProfileDetails(string RegistrationID)
        {
            var result = EventAccessLayer.FetchUserProfileDetailsById(RegistrationID);
            if (result.Rows.Count > 0)
            {
                var DataList = new List<RegistrationModel>();
                foreach (DataRow dr in result.Rows)
                {
                    RegistrationModel Obj = new RegistrationModel();
                    Obj.Username = Convert.ToString(dr["Username"]);
                    Obj.Mobileno = Convert.ToString(dr["Mobileno"]);
                    Obj.EmailID = Convert.ToString(dr["EmailID"]);
                    Obj.Gender = Convert.ToString(dr["Gender"]);
                    Obj.Birthdate = Convert.ToDateTime(dr["Birthdate"]);
                    Obj.Country = Convert.ToString(dr["Country"]);
                    Obj.State = Convert.ToString(dr["State"]);
                    Obj.City = Convert.ToString(dr["City"]);
                    Obj.Address = Convert.ToString(dr["Address"]);
                    Obj.CreatedOn = Convert.ToDateTime(dr["CreatedOn"]);

                    DataList.Add(Obj);
                }
                return Json(new { IsSuccess = true, Message = "", DataList = DataList.FirstOrDefault() });
            }
            else
            {
                return Json(new { IsSuccess = false, Message = "No records are available for this master" });
            }
        }


    }
}
