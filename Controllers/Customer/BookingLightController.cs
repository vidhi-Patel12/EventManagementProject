using Event.Models.AccessLayer.CustomerAccessLayer;
using Event.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Data;

namespace Event.Controllers.Customer
{
    public class BookingLightController : Controller
    {
        public IActionResult Success()
        {
            var Name = HttpContext.Session.GetString("Name");
            ViewBag.Name = Name;
            return View();
            
        }
        public IActionResult BookLight()
        {
            var Name = HttpContext.Session.GetString("Name");
            ViewBag.Name = Name;
            return View();
        }

        [HttpPost]
        public JsonResult BookLight(string BookLightlst)
        {
            var uploadDirectory = Path.Combine("wwwroot", "/LightImages/");
            Directory.CreateDirectory(uploadDirectory);
            string[]? bookLight = JsonConvert.DeserializeObject<string[]>(BookLightlst);
            //string[]? Lightitem = JsonConvert.DeserializeObject<string[]>(Lightitem);
            var resultAdd = BookingLightAccessLayer.BookingLight(bookLight);
            if (resultAdd)
            {
                return Json(new { IsSuccess = true, Message = "Booking Venue added sucessfully" });
            }
            else
            {
                return Json(new { IsSuccess = false, Message = "Something went wrong while booking Venue" });
            }

        }

        public JsonResult GetLightList()
        {

            DataTable dt = BookingLightAccessLayer.GetAllLight();

            List<LightModel> list = new List<LightModel>();

            foreach (DataRow dr in dt.Rows)
            {
                string lightName = dr["LightName"].ToString();
                string lightID = dr["LightID"].ToString();
                string lightFilePath = dr["LightFilePath"].ToString();


                list.Add(new LightModel
                {
                    LightName = lightName,
                    LightID = Convert.ToInt32(lightID),
                    LightFilePath = lightFilePath

                });

            }
            return Json(new { list });

        }

        public JsonResult BookingID()
        {
            try
            {

                var resultAdd = BookingLightAccessLayer.BookingID();

                if (resultAdd.Rows.Count > 0)
                {
                    var DataList = new List<BookingDetailModel>();
                    foreach (DataRow dr in resultAdd.Rows)
                    {
                        BookingDetailModel Obj = new BookingDetailModel();
                        Obj.BookingID = Convert.ToInt32(dr["BookingID"]);
                        Obj.Createdby = Convert.ToInt32(dr["Createdby"]);
                        DataList.Add(Obj);
                    }



                    var data = DataList.LastOrDefault();
                    var bookingno = data.BookingID;
                    var createdbyid = data.Createdby;
                    //BookingVenueAccessLayer.BookingNousingID(bookingno);

                    HttpContext.Session.SetInt32("BookingId", bookingno);
                    HttpContext.Session.SetInt32("CreatedById", createdbyid);
                    SessionClass.BookingId = bookingno;
                    SessionClass.CreatedById = createdbyid;
                }
                else
                {
                    return Json(new { IsSuccess = false, RedirectUrl = "/Registration/Create" });
                }
            }
            catch
            {
                return Json(new { IsSuccess = false, Message = "An unexpected error occurred" });
            }
            return Json(new { IsSuccess = false, Message = "An unexpected error occurred" });
        }

    }
}
