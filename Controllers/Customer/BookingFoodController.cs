using Event.Models;
using Event.Models.AccessLayer.CustomerAccessLayer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Data;

namespace Event.Controllers.Customer
{
    public class BookingFoodController : Controller
    {
        public IActionResult Success()
        {
            var Name = HttpContext.Session.GetString("Name");
            ViewBag.Name = Name;
            return View();
            
        }

        public IActionResult BookFood()
        {
            var Name = HttpContext.Session.GetString("Name");
            ViewBag.Name = Name;
            return View();
        }
        [HttpPost]
        public JsonResult BookFood(string BookFoodlst , string Fooditem)
        {
            var uploadDirectory = Path.Combine("wwwroot", "/FoodImages/");
            Directory.CreateDirectory(uploadDirectory);
            string[]? bookFood = JsonConvert.DeserializeObject<string[]>(BookFoodlst);
            string[]? fooditem = JsonConvert.DeserializeObject<string[]>(Fooditem);
            var resultAdd = BookingFoodAccessLayer.BookingFood(bookFood, fooditem);
            if (resultAdd)
            {
                return Json(new { IsSuccess = true, Message = "Booking Venue added sucessfully" });
            }
            else
            {
                return Json(new { IsSuccess = false, Message = "Something went wrong while booking Venue" });
            }

        }

        public JsonResult GetFoodList()
        {

            DataTable dt = BookingFoodAccessLayer.GetAllFood();

            List<FoodModel> list = new List<FoodModel>();

            foreach (DataRow dr in dt.Rows)
            {
                string foodName = dr["FoodName"].ToString();
                string foodID = dr["FoodID"].ToString();
                string foodFilePath = dr["FoodFilePath"].ToString();


                list.Add(new FoodModel
                {
                    FoodName = foodName,
                    FoodID = Convert.ToInt32(foodID),
                    FoodFilePath = foodFilePath

                });

            }
            return Json(new { list });

        }

        public JsonResult BookingID()
        {
            try
            {

                var resultAdd = BookingFoodAccessLayer.BookingID();

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
