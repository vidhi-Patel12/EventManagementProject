using Event.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting.Internal;
using Newtonsoft.Json;
using NuGet.Protocol.Plugins;
using System.Data;



namespace Event.Controllers.Venue
{
    public class VenueController : Controller
    {



        public IActionResult VenueIndex()
        {
            var Name = HttpContext.Session.GetString("Name");
            ViewBag.Name = Name;
            return View();
        }



        [HttpGet]
        public IActionResult AddVenue()
        {

            return View();
        }

        [HttpPost]
        public async Task<ActionResult> AddUpdateVenue(IFormFile VenueFilepath, [FromServices] IWebHostEnvironment hostingEnvironment)
        {
            try
            {
                string[] venuelist = HttpContext.Request.Form["venuelist[]"];

                //var uploadDirectory = Path.Combine("wwwroot", "VenuImages");
                var uploadDirectory = Path.Combine(hostingEnvironment.WebRootPath, "VenueImages");

                Directory.CreateDirectory(uploadDirectory);

                if (VenueFilepath != null && VenueFilepath.Length > 0)
                {
                    var fileName = Path.GetFileName(VenueFilepath.FileName);
                    var filePathInDatabase = Path.Combine("/VenueImages", fileName);

                    if (!string.IsNullOrEmpty(venuelist[0]))
                    {
                        var result = EventAccessLayer.FetchVenueUsingId(venuelist[0]);
                        if (result.Rows.Count > 0)
                        {
                            var resultUpdate = EventAccessLayer.EditVenue(venuelist, VenueFilepath, uploadDirectory, filePathInDatabase);
                            var filePath = Path.Combine(uploadDirectory, fileName);
                            using (var fileStream = new FileStream(filePath, FileMode.Create))
                            {
                                await VenueFilepath.CopyToAsync(fileStream);
                            }
                            if (resultUpdate)
                            {
                                return Json(new { IsSuccess = true, Message = "Data update success" });
                            }
                            else
                            {
                                return Json(new { IsSuccess = false, Message = "Something went wrong while updating details" });
                            }
                        }

                        else
                        {
                            return Json(new { IsSuccess = false, Message = "Error Occurred" });
                        }

                    }
                    else
                    {
                        var resultAdd = EventAccessLayer.AddVenue(venuelist, VenueFilepath, uploadDirectory, filePathInDatabase, HttpContext);
                        var filePath = Path.Combine(uploadDirectory, fileName);
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await VenueFilepath.CopyToAsync(fileStream);
                        }
                        if (resultAdd)
                        {
                            return Json(new { IsSuccess = true, Message = "Success" });
                        }
                        else
                        {
                            return Json(new { IsSuccess = false, Message = "Error Occurred" });
                        }
                    }

                }
                else
                {
                    return Json(new { IsSuccess = false, Message = "Plese Upload A File" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { IsSuccess = false, Message = "An unexpected error occurred" });
            }
            return Json(new { IsSuccess = false, Message = "Invalid request" });
        }



        public ActionResult LoadVenue()
        {
            var result = EventAccessLayer.fetchVenue();
            if (result.Rows.Count > 0)
            {
                var DataList = new List<VenueModel>();
                foreach (DataRow dr in result.Rows)
                {
                    VenueModel Obj = new VenueModel();
                    Obj.VenueID = Convert.ToInt32(dr["VenueID"]);
                    Obj.VenueName = Convert.ToString(dr["VenueName"]);
                    Obj.VenueCost = Convert.ToInt32(dr["VenueCost"]);
                    Obj.VenueFilename = Convert.ToString(dr["VenueFileName"]);
                    Obj.VenueFilePath = Convert.ToString(dr["VenueFilePath"]);
                    Obj.Createdate = Convert.ToDateTime(dr["Createdate"]);
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
        public async Task<ActionResult> LoadVenueDetails(string VenueID)
        {
            var result = EventAccessLayer.FetchVenueDetailsById(VenueID);
            if (result.Rows.Count > 0)
            {
                var DataList = new List<VenueModel>();
                foreach (DataRow dr in result.Rows)
                {
                    VenueModel Obj = new VenueModel();
                    Obj.VenueID = Convert.ToInt32(dr["VenueID"]);
                    Obj.VenueName = Convert.ToString(dr["VenueName"]);
                    Obj.VenueCost = Convert.ToInt32(dr["VenueCost"]);
                    Obj.Createdate = Convert.ToDateTime(dr["Createdate"]);

                    DataList.Add(Obj);
                }
                return Json(new { IsSuccess = true, Message = "", DataList = DataList.FirstOrDefault() });
            }
            else
            {
                return Json(new { IsSuccess = false, Message = "No records are available for this master" });
            }
        }

        public ActionResult LoadEditVenueData(string VenueID)
        {


            var result = EventAccessLayer.FetchVenueDetailsById(VenueID);
            if (result.Rows.Count > 0)
            {
                var DataList = new List<VenueModel>();
                foreach (DataRow dr in result.Rows)
                {
                    VenueModel Obj = new VenueModel();
                    Obj.VenueID = Convert.ToInt32(dr["VenueID"]);
                    Obj.VenueName = Convert.ToString(dr["VenueName"]);
                    Obj.VenueCost = Convert.ToInt32(dr["VenueCost"]);
                    Obj.VenueFilename = Convert.ToString(dr["VenueFilename"]);
                    Obj.VenueFilePath = Convert.ToString(dr["VenueFilePath"]);
                    Obj.Createdate = Convert.ToDateTime(dr["Createdate"]);

                    DataList.Add(Obj);
                }
                return Json(new { IsSuccess = true, Message = "", DataList = DataList.FirstOrDefault() });
            }
            else
            {
                return Json(new { IsSuccess = false, Message = "No records are available for this master" });
            }
        }



        [HttpPost]
        public ActionResult DeleteVenueData(string VenueID, [FromServices] IWebHostEnvironment hostingEnvironment)
        {
            try
            {
                var dataTable = EventAccessLayer.FetchVenueDetailsById(VenueID);

                string wwwRootPath = hostingEnvironment.WebRootPath;

                string venueImagesFolderPath = Path.Combine(wwwRootPath);

                if (dataTable != null && dataTable.Rows.Count > 0)
                {
                    // Access the VenueFilePath from the first row of the DataTable
                    string venueFileName = dataTable.Rows[0]["VenueFilePath"].ToString().Replace("/", "\\");

                    // Combine the folder path with the file name
                    string venueFilePath = Path.Combine(venueImagesFolderPath, venueFileName.TrimStart('\\'));

                    if (System.IO.File.Exists(venueFilePath))
                    {
                        // Delete file from the filesystem
                        System.IO.File.Delete(venueFilePath);

                        // Delete record from the SQL table
                        var result = EventAccessLayer.DeleteVenueData(VenueID);
                        if (result)
                        {
                            return Json(new { IsSuccess = true, Message = "Record is deleted!" });
                        }
                        else
                        {
                            return Json(new { IsSuccess = false, Message = "Something went wrong while deleting record!" });
                        }
                    }
                    else
                    {
                        return Json(new { IsSuccess = false, Message = "File not found for the specified VenueID!" });
                    }
                }
                else
                {
                    return Json(new { IsSuccess = false, Message = "No record found for the specified VenueID!" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { IsSuccess = false, Message = "Error occurred: " + ex.Message });
            }
        }






    }

}

