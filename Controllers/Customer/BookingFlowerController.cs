using Event.Models.AccessLayer.CustomerAccessLayer;
using Event.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Data;

namespace Event.Controllers.Customer
{
    public class BookingFlowerController : Controller
    {
        public IActionResult Success()
        {
            var Name = HttpContext.Session.GetString("Name");
            ViewBag.Name = Name;
            return View();
           
        }
        public IActionResult BookFlower()
        {
            var Name = HttpContext.Session.GetString("Name");
            ViewBag.Name = Name;
            return View();
        }

        [HttpPost]
        public JsonResult BookFlower(string BookFlowerlst)
        {
            var uploadDirectory = Path.Combine("wwwroot", "/FlowerImages/");
            Directory.CreateDirectory(uploadDirectory);
            string[]? bookFlower = JsonConvert.DeserializeObject<string[]>(BookFlowerlst);
            //string[]? Floweritem = JsonConvert.DeserializeObject<string[]>(Floweritem);
            var resultAdd = BookingFlowerAccessLayer.BookingFlower(bookFlower);
            if (resultAdd)
            {
                return Json(new { IsSuccess = true, Message = "Booking Venue added sucessfully" });
            }
            else
            {
                return Json(new { IsSuccess = false, Message = "Something went wrong while booking Venue" });
            }

        }

        public JsonResult GetFlowerList()
        {

            DataTable dt = BookingFlowerAccessLayer.GetAllFlower();

            List<FlowerModel> list = new List<FlowerModel>();

            foreach (DataRow dr in dt.Rows)
            {
                string flowerName = dr["FlowerName"].ToString();
                string flowerID = dr["FlowerID"].ToString();
                string flowerFilePath = dr["FlowerFilePath"].ToString();


                list.Add(new FlowerModel
                {
                    FlowerName = flowerName,
                    FlowerID = Convert.ToInt32(flowerID),
                    FlowerFilePath = flowerFilePath

                });

            }
            return Json(new { list });

        }

        public JsonResult BookingID()
        {
            try
            {

                var resultAdd = BookingFlowerAccessLayer.BookingID();

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
