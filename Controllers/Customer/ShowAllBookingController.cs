using Event.Models;
using Event.Models.AccessLayer.AdminAccessLayer;
using Event.Models.AccessLayer.CustomerAccessLayer;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data;

namespace Event.Controllers.Customer
{
    public class ShowAllBookingController : Controller
    {
        public IActionResult AllBooking()
        {
            var Name = HttpContext.Session.GetString("Name");
            ViewBag.Name = Name;
            return View();
        }

        [HttpPost]
        public IActionResult LoadBookingdetails()
        {
            try
            {
                var result = ShowAllBookingAccessLayer.GetBookingDetails();
                if (result.Rows.Count > 0)
                {
                    var List = new List<BookingDetailModel>();
                    foreach (DataRow dr in result.Rows)
                    {
                        BookingDetailModel Obj = new BookingDetailModel();
                        Obj.Createdby = Convert.ToInt32(dr["Createdby"]);
                        Obj.BookingID = Convert.ToInt32(dr["BookingID"]);
                        Obj.BookingNo = Convert.ToString(dr["BookingNo"]);
                        Obj.BookingDate = Convert.ToDateTime(dr["BookingDate"]);
                        Obj.BookingApproval = Convert.ToString(dr["BookingApproval"]);
                        Obj.BookingApprovalDate = dr["BookingApprovalDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["BookingApprovalDate"]);

                        List.Add(Obj);
                    }
                    return Json(new { IsSuccess = true, Message = "", List });
                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }
            return Json(new { IsSuccess = false, Message = "Something went wrong while fetching records" });
        }

        [HttpGet]
        public IActionResult Getshowallbooking(string BookingNo)
        {
            try
            {

                var venueResult = ShowAllBookingAccessLayer.GetVenuecost(BookingNo);
                string venueName = "";
                List<int> VenueCost = new List<int>();
                int totalVenueCost = 0;
                if (venueResult.Rows.Count > 0)
                {
                    venueName= Convert.ToString(venueResult.Rows[0]["VenueName"]);
                    int venueCost = Convert.ToInt32(venueResult.Rows[0]["VenueCost"]);
                    VenueCost.Add(venueCost);
                    totalVenueCost += venueCost;
                }

                var EquipmentCostResult = ShowAllBookingAccessLayer.GetEquipmentcost(BookingNo);
                string equipmentName = "";
                List<int> EquipmentCost = new List<int>();
                int totalEquipmentCost = 0;
                foreach (DataRow row in EquipmentCostResult.Rows)
                {
                    equipmentName = Convert.ToString(row["EquipmentName"]);
                    int equipmentCost = Convert.ToInt32(row["EquipmentCost"]);
                    EquipmentCost.Add(equipmentCost);
                    totalEquipmentCost += equipmentCost;
                }


                var FoodResult = ShowAllBookingAccessLayer.GetFoodcost(BookingNo);
                string foodName = "";
                string foodType = "";
                string mealType = "";
                List<int> Foodcost = new List<int>();
                int totalFoodCost = 0;
                foreach (DataRow row in FoodResult.Rows)
                {
                    foodName = Convert.ToString(row["FoodName"]);
                    foodType = Convert.ToString(row["FoodType"]);
                    mealType = Convert.ToString(row["MealType"]);
                    int foodCost = Convert.ToInt32(row["FoodCost"]);
                    Foodcost.Add(foodCost);
                    totalFoodCost += foodCost;
                }

                var LightResult = ShowAllBookingAccessLayer.GetLightCost(BookingNo);
                string lightName = "";
                string lightType = "";
                List<int> Lighhtcost = new List<int>();
                int totalLightCost = 0;
                foreach (DataRow row in LightResult.Rows)
                {
                    lightName = Convert.ToString(row["LightName"]);
                    lightType = Convert.ToString(row["LightType"]);
                    int lightCost = Convert.ToInt32(row["LightCost"]);
                    Lighhtcost.Add(lightCost);
                    totalLightCost += lightCost;
                }

                var flowerResult = ShowAllBookingAccessLayer.GetFlowerCost(BookingNo);
                string flowerName = "";
                List<int> flowercost = new List<int>();
                int totalFlowerCost = 0;
                foreach (DataRow row in flowerResult.Rows)
                {
                    flowerName = Convert.ToString(row["FlowerName"]);
                    int flowerCost = Convert.ToInt32(row["FlowerCost"]);
                    flowercost.Add(flowerCost);
                    totalFlowerCost += flowerCost;
                }

                var bookingdetails = ShowAllBookingAccessLayer.GetbookingdetailsbyBookingNo(BookingNo);
                string bookingNo = string.Empty;
                DateTime bookingDate = DateTime.MinValue;

                if (bookingdetails.Rows.Count > 0)
                {
                    bookingNo = Convert.ToString(bookingdetails.Rows[0]["BookingNo"]);

                    string bookingDateString = Convert.ToString(bookingdetails.Rows[0]["BookingDate"]);
                    bookingDate = DateTime.Parse(bookingDateString);
                }

                var model = new
                {
                    IsSuccess = true,
                    VenueName= venueName,
                    VenueCost = totalVenueCost,
                    EquipmentName = equipmentName,
                    EquipmentCost = totalEquipmentCost,
                    FoodName = foodName,
                    FoodType= foodType,
                    MealType= mealType,
                    FoodCost = totalFoodCost,
                    LightName = lightName,
                    LightType = lightType,
                    LightCost = totalLightCost,
                    FlowerName = flowerName,
                    FlowerCost = totalFlowerCost,
                    BookingNo = bookingNo,
                    BookingDate = bookingDate,
                };

                return Json(model);
            }
            catch (Exception ex)
            {
                return Json(new { IsSuccess = false, Message = ex.Message });
            }
        }



        [HttpPost]
        public async Task<ActionResult> BookingCanceled(string DATA)
        {
            try
            {
                string[]? Status = JsonConvert.DeserializeObject<string[]>(DATA);

                var resultAdd = ShowAllBookingAccessLayer.Cancel(Status);

                if (resultAdd)
                {
                    return Json(new { IsSuccess = true, Message = "Rejected" });
                }
                else
                {
                    return Json(new { IsSuccess = false, Message = "An Error Occured" });
                }
            }
            catch (Exception)
            {
                return Json(new { IsSuccess = false, Message = "An unexpected error occurred" });
            }
        }
    }
}
