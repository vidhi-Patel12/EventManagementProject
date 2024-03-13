using Event.Models;
using Event.Models.AccessLayer.AdminAccessLayer;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data;

namespace Event.Controllers.Equipment
{
    public class EquipmentController : Controller
    {
        public ActionResult EquipmentIndex()
        {
            var Name = HttpContext.Session.GetString("Name");
            ViewBag.Name = Name;
            return View();

        }

        [HttpGet]
        public IActionResult AddEquipment()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> AddUpdateEquipment(IFormFile EquipmentFilepath, [FromServices] IWebHostEnvironment hostingEnvironment)
        {
            try
            {
                string[] equipmentlist = HttpContext.Request.Form["equipmentlist[]"];

                //var uploadDirectory = Path.Combine("wwwroot", "VenuImages");
                var uploadDirectory = Path.Combine(hostingEnvironment.WebRootPath, "EquipmentImages");

                Directory.CreateDirectory(uploadDirectory);

                if (EquipmentFilepath != null && EquipmentFilepath.Length > 0)
                {
                    var fileName = Path.GetFileName(EquipmentFilepath.FileName);
                    var filePathInDatabase = Path.Combine("/EquipmentImages", fileName);

                    if (!string.IsNullOrEmpty(equipmentlist[0]))
                    {
                        var result = EquipmentAccessLayer.FetchEquipmentUsingId(equipmentlist[0]);
                        if (result.Rows.Count > 0)
                        {
                            var resultUpdate = EquipmentAccessLayer.EditEquipment(equipmentlist, EquipmentFilepath, uploadDirectory, filePathInDatabase,HttpContext);
                            var filePath = Path.Combine(uploadDirectory, fileName);
                            using (var fileStream = new FileStream(filePath, FileMode.Create))
                            {
                                await EquipmentFilepath.CopyToAsync(fileStream);
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
                        var resultAdd = EquipmentAccessLayer.AddEquipment(equipmentlist, EquipmentFilepath, uploadDirectory, filePathInDatabase, HttpContext);
                        var filePath = Path.Combine(uploadDirectory, fileName);
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await EquipmentFilepath.CopyToAsync(fileStream);
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



        public ActionResult LoadEquipment()
        {
            var result = EquipmentAccessLayer.fetchEquipment();
            if (result.Rows.Count > 0)
            {
                var DataList = new List<EquipmentModel>();
                foreach (DataRow dr in result.Rows)
                {
                    EquipmentModel Obj = new EquipmentModel();
                    Obj.EquipmentID = Convert.ToInt32(dr["EquipmentID"]);
                    Obj.EquipmentName = Convert.ToString(dr["EquipmentName"]);
                    Obj.EquipmentCost = Convert.ToInt32(dr["EquipmentCost"]);
                    Obj.EquipmentFilename = Convert.ToString(dr["EquipmentFilename"]);
                    Obj.EquipmentFilePath = Convert.ToString(dr["EquipmentFilePath"]);
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



        [HttpGet]
        public async Task<ActionResult> LoadEquipmentDetails(string EquipmentID)
        {
            var result = EquipmentAccessLayer.FetchEquipmentDetailsById(EquipmentID);
            if (result.Rows.Count > 0)
            {
                var DataList = new List<EquipmentModel>();
                foreach (DataRow dr in result.Rows)
                {
                    EquipmentModel Obj = new EquipmentModel();
                    Obj.EquipmentID = Convert.ToInt32(dr["EquipmentID"]);
                    Obj.EquipmentName = Convert.ToString(dr["EquipmentName"]);
                    Obj.EquipmentCost = Convert.ToInt32(dr["EquipmentCost"]);
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

        public ActionResult LoadEditEquipmentData(string EquipmentID)
        {
            var result = EquipmentAccessLayer.FetchEquipmentDetailsById(EquipmentID);
            if (result.Rows.Count > 0)
            {
                var DataList = new List<EquipmentModel>();
                foreach (DataRow dr in result.Rows)
                {
                    EquipmentModel Obj = new EquipmentModel();
                    Obj.EquipmentID = Convert.ToInt32(dr["EquipmentID"]);
                    Obj.EquipmentName = Convert.ToString(dr["EquipmentName"]);
                    Obj.EquipmentCost = Convert.ToInt32(dr["EquipmentCost"]);
                    Obj.EquipmentFilename = Convert.ToString(dr["EquipmentFilename"]);
                    Obj.EquipmentFilePath = Convert.ToString(dr["EquipmentFilePath"]);
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
        public ActionResult DeleteEquipmentData(string EquipmentID, [FromServices] IWebHostEnvironment hostingEnvironment)
        {
            try
            {
                var dataTable = EquipmentAccessLayer.FetchEquipmentDetailsById(EquipmentID);

                string wwwRootPath = hostingEnvironment.WebRootPath;

                string equipmentImagesFolderPath = Path.Combine(wwwRootPath);

                if (dataTable != null && dataTable.Rows.Count > 0)
                {
                    // Access the VenueFilePath from the first row of the DataTable
                    string equipmentFileName = dataTable.Rows[0]["EquipmentFilePath"].ToString().Replace("/", "\\");

                    // Combine the folder path with the file name
                    string equipmentFilePath = Path.Combine(equipmentImagesFolderPath, equipmentFileName.TrimStart('\\'));

                    if (System.IO.File.Exists(equipmentFilePath))
                    {
                        // Delete file from the filesystem
                        System.IO.File.Delete(equipmentFilePath);

                        // Delete record from the SQL table
                        var result = EquipmentAccessLayer.DeleteEquipmentData(EquipmentID);
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
