using Event.Models;
using Event.Models.AccessLayer.CustomerAccessLayer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Data;

namespace Event.Controllers.Customer
{
    public class BookingEquipmentController : Controller
    {
        public ActionResult Success()
        {
            var Name = HttpContext.Session.GetString("Name");
            ViewBag.Name = Name;
            return View();
            
        }
        public IActionResult BookingEquipment()
        {
            var Name = HttpContext.Session.GetString("Name");
            ViewBag.Name = Name;
            return View();
        }

        [HttpPost]
        public JsonResult BookingEquipment(string BookEquipmentlst)
        {
            var uploadDirectory = Path.Combine("wwwroot", "/EquipmentImages/");
            Directory.CreateDirectory(uploadDirectory);

            string[]? bookequipment = JsonConvert.DeserializeObject<string[]>(BookEquipmentlst);
            var resultAdd = BookingEquipmentAccessLayer.BookingEquipment(bookequipment);
            if (resultAdd)
            {
                return Json(new { IsSuccess = true, Message = "Booking Equipment added sucessfully" });
            }
            else
            {
                return Json(new { IsSuccess = false, Message = "Something went wrong while booking Venue" });
            }
            
        }

        public JsonResult GetEquipmentList()
        {

            DataTable dt = BookingEquipmentAccessLayer.GetAllEquipment();

            List<EquipmentModel> list = new List<EquipmentModel>();

            foreach (DataRow dr in dt.Rows)
            {
                string equipmentName = dr["EquipmentName"].ToString();
                string equipmentID = dr["EquipmentID"].ToString();
                string equipmentFilePath = dr["EquipmentFilePath"].ToString();


                list.Add(new EquipmentModel
                {
                    EquipmentName = equipmentName,
                    EquipmentID = Convert.ToInt32(equipmentID),
                    EquipmentFilePath = equipmentFilePath

                });

            }
            return Json(new { list });

        }


        public JsonResult BookingID()
        {
            try
            {
                
                var resultAdd = BookingEquipmentAccessLayer.BookingID();

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
