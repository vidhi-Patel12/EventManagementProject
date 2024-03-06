using Event.Models;
using Event.Models.AccessLayer.CustomerAccessLayer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Data;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace Event.Controllers.Customer
{
    public class BookingVenueController : Controller
    {
        public ActionResult Success()
        {
            return View();
        }

        public JsonResult BookingDetailsForID()
        {
            try
            {
                //HttpContext.Session.SetString("ID", 1);

                //Console.WriteLine(HttpContext.Session.SetString("userid"));

                //string roleid = EventAccessLayer.FetchUserProfileDetailsById(RegistrationID);
                var resultAdd = BookingVenueAccessLayer.BookingDetailsID();

                if (resultAdd.Rows.Count > 0)
                {
                    var DataList = new List<BookingDetailModel>();
                    foreach (DataRow dr in resultAdd.Rows)
                    {
                        BookingDetailModel Obj = new BookingDetailModel();
                        Obj.BookingID = Convert.ToInt32(dr["BookingID"]);
                        //Obj.ID = Convert.ToInt32(dr["ID"]);
                        DataList.Add(Obj);
                    }



                    var data = DataList.LastOrDefault();
                    var bookingno = data.BookingID;
                    BookingVenueAccessLayer.BookingNousingID(bookingno);

                    HttpContext.Session.SetInt32("BookingId", bookingno);
                    SessionClass.BookingId = bookingno;

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

        public ActionResult BookingDetails(string BookVenuelst)
        {
            string[]? bookdetails = JsonConvert.DeserializeObject<string[]>(BookVenuelst);
            var resultAdd = BookingVenueAccessLayer.BookingDetails(bookdetails);
            if (resultAdd)
            {
                return Json(new { IsSuccess = true, Message = "Booking Venue added sucessfully", BookingNo = BookVenuelst[1] });
            }
            else
            {
                return Json(new { IsSuccess = false, Message = "Something went wrong while booking Venue" });
            }
        }



        public IActionResult BookVenue()
        {
            var Name = HttpContext.Session.GetString("Name");
            ViewBag.Name = Name;
            return View();
        }
        [HttpPost]
        public JsonResult BookVenue(string BookVenuelst)
        {
            var uploadDirectory = Path.Combine("wwwroot", "/VenueImages/");
            Directory.CreateDirectory(uploadDirectory);
            // var fileName = Path.GetFileName(VenueFilePath.FileName);
            // var filePathInDatabase = Path.Combine(uploadDirectory, fileName);
            string[]? bookvenue = JsonConvert.DeserializeObject<string[]>(BookVenuelst);
            var resultAdd = BookingVenueAccessLayer.BookingVenue(bookvenue);
            if (resultAdd)
            {
                return Json(new { IsSuccess = true, Message = "Booking Venue added sucessfully", BookingVenueID = BookVenuelst[0] });
            }
            else
            {
                return Json(new { IsSuccess = false, Message = "Something went wrong while booking Venue" });
            }
        }

        public JsonResult GetEventList()
        {

            DataTable dt = BookingVenueAccessLayer.GetAllEvent();

            List<SelectListItem> list = new List<SelectListItem>();

            foreach (DataRow dr in dt.Rows)
            {
                list.Add(new SelectListItem
                {
                    Text = dr["EventType"].ToString(),
                    Value = dr["EventID"].ToString(),
                });
            }
            return Json(new { list });

        }
        public JsonResult GetVenueList()
        {

            DataTable dt = BookingVenueAccessLayer.GetAllVenue();

            List<VenueModel> list = new List<VenueModel>();

            foreach (DataRow dr in dt.Rows)
            {
                string venueName = dr["VenueName"].ToString();
                string venueID = dr["VenueID"].ToString();
                string venueFilePath = dr["VenueFilePath"].ToString();


                list.Add(new VenueModel
                {
                    VenueName = venueName,
                    VenueID = Convert.ToInt32(venueID),
                    VenueFilePath = venueFilePath

                });

            }

            return Json(new { list });
        }

        [HttpPost]
        public ActionResult CheckBookingAvailability(DateTime bookingDate, int venueID)
        {
            string bookingStatus = IsBookingAvailable(bookingDate, venueID);

            if (bookingStatus == "Date is already booked for the same venue.")
            {
                return Json(new { success = false, errorMessage = bookingStatus });
            }
            else
            {
                return Json(new { success = true });
            }
        }

        private string IsBookingAvailable(DateTime bookingDate, int venueID)
        {
            string bookingStatus = BookingVenueAccessLayer.IsVenueBookedOnDate(bookingDate, venueID);
            return bookingStatus;
        }
    }

}

