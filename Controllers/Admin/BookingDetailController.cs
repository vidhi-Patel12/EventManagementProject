using Event.Models;
using Event.Models.AccessLayer.AdminAccessLayer;
using Event.Utility;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data;

namespace Event.Controllers.Admin
{
    public class BookingDetailController : Controller
    {
        private static string connectionString = ConnectionString.Get("Connection");
        public IActionResult BookingDetail()
        {
            var Name = HttpContext.Session.GetString("Name");
            ViewBag.Name = Name;
            return View();
        }


        [HttpPost]
        public async Task<ActionResult> BookingApproved(string DATA)
        {
            try
            {
                string[]? Status = JsonConvert.DeserializeObject<string[]>(DATA);

                var resultAdd = BookingDetailAccessLayer.Approved(Status);

                if (resultAdd)
                {
                    return Json(new { IsSuccess = true, Message = "Approved" });
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

        [HttpPost]
        public async Task<ActionResult> BookingRejected(string DATA)
        {
            try
            {
                string[]? Status = JsonConvert.DeserializeObject<string[]>(DATA);

                var resultAdd = BookingDetailAccessLayer.Reject(Status);

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

        [HttpPost]
        public IActionResult LoadBookingdetails()
        {
            try
            {
                var result = BookingDetailAccessLayer.GetBookingDetails();
                if (result.Rows.Count > 0)
                {
                    var List = new List<BookingDetailModel>();
                    foreach (DataRow dr in result.Rows)
                    {
                        BookingDetailModel Obj = new BookingDetailModel();
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
        public ActionResult Getdataforedit(string BookingNo)
        {
            try
            {


                var venueResult = BookingDetailAccessLayer.GetVenuecost(BookingNo);
                List<int> VenueCost = new List<int>();
                int totalVenueCost = 0;
                if (venueResult.Rows.Count > 0)
                {
                    //venueCost = Convert.ToInt32(venueResult.Rows[0]["VenueCost"]);
                    int venueCost = Convert.ToInt32(venueResult.Rows[0]["VenueCost"]);
                    VenueCost.Add(venueCost);
                    totalVenueCost += venueCost;
                }

                var EquipmentCostResult = BookingDetailAccessLayer.GetEquipmentcost(BookingNo);
                List<int> EquipmentCost = new List<int>();
                int totalEquipmentCost = 0;
                foreach (DataRow row in EquipmentCostResult.Rows)
                {
                    //EquipmentCost.Add(Convert.ToInt32(row["EquipmentCost"]));
                    int equipmentCost = Convert.ToInt32(row["EquipmentCost"]);
                    EquipmentCost.Add(equipmentCost);
                    totalEquipmentCost += equipmentCost;
                }


                var FoodResult = BookingDetailAccessLayer.GetFoodcost(BookingNo);
                List<int> Foodcost = new List<int>();
                int totalFoodCost = 0;
                foreach (DataRow row in FoodResult.Rows)
                {
                    //Foodcost.Add(Convert.ToInt32(row["FoodCost"]));
                    int foodCost = Convert.ToInt32(row["FoodCost"]);
                    Foodcost.Add(foodCost); 
                    totalFoodCost += foodCost;
                }

                var LightResult = BookingDetailAccessLayer.GetLightCost(BookingNo);
                List<int> Lighhtcost = new List<int>();
                int totalLightCost = 0;
                foreach (DataRow row in LightResult.Rows)
                {
                    //Lighhtcost.Add(Convert.ToInt32(row["Lightcost"]));
                    int lightCost = Convert.ToInt32(row["LightCost"]);
                    Lighhtcost.Add(lightCost);
                    totalLightCost += lightCost;
                }

                var flowerResult = BookingDetailAccessLayer.GetFlowerCost(BookingNo);
                List<int> flowercost = new List<int>();
                int totalFlowerCost = 0;
                foreach (DataRow row in flowerResult.Rows)
                {
                    //flowercost.Add(Convert.ToInt32(row["FlowerCost"]));
                    int flowerCost = Convert.ToInt32(row["FlowerCost"]);
                    flowercost.Add(flowerCost);
                    totalFlowerCost += flowerCost;
                }

                var bookingdetails = BookingDetailAccessLayer.Getbookingdetails(BookingNo);
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
                    VenueCost = totalVenueCost,
                    EquipmentCost = totalEquipmentCost,
                    FoodCost = totalFoodCost,
                    LightCost = totalLightCost,
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

    }
}
