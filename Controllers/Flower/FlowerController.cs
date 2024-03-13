using Event.Models;
using Event.Models.AccessLayer.AdminAccessLayer;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace Event.Controllers.Flower
{
    public class FlowerController : Controller
    {
        public ActionResult FlowerIndex()
        {
            var Name = HttpContext.Session.GetString("Name");
            ViewBag.Name = Name;
            return View();
        }

        [HttpGet]
        public IActionResult AddFlower()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> AddUpdateFlower(IFormFile FlowerFilepath, [FromServices] IWebHostEnvironment hostingEnvironment)
        {
            try
            {
                string[] flowerlist = HttpContext.Request.Form["flowerlist[]"];

                //var uploadDirectory = Path.Combine("wwwroot", "VenuImages");
                var uploadDirectory = Path.Combine(hostingEnvironment.WebRootPath, "FlowerImages");

                Directory.CreateDirectory(uploadDirectory);

                if (FlowerFilepath != null && FlowerFilepath.Length > 0)
                {
                    var fileName = Path.GetFileName(FlowerFilepath.FileName);
                    var filePathInDatabase = Path.Combine("/FlowerImages", fileName);

                    if (!string.IsNullOrEmpty(flowerlist[0]))
                    {
                        var result = FlowerAccessLayer.FetchFlowerUsingId(flowerlist[0]);
                        if (result.Rows.Count > 0)
                        {
                            var resultUpdate = FlowerAccessLayer.EditFlower(flowerlist, FlowerFilepath, uploadDirectory, filePathInDatabase,HttpContext);
                            var filePath = Path.Combine(uploadDirectory, fileName);
                            using (var fileStream = new FileStream(filePath, FileMode.Create))
                            {
                                await FlowerFilepath.CopyToAsync(fileStream);
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
                        var resultAdd = FlowerAccessLayer.AddFlower(flowerlist, FlowerFilepath, uploadDirectory, filePathInDatabase, HttpContext);
                        var filePath = Path.Combine(uploadDirectory, fileName);
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await FlowerFilepath.CopyToAsync(fileStream);
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



        public ActionResult LoadFlower()
        {
            var result = FlowerAccessLayer.fetchFlower();
            if (result.Rows.Count > 0)
            {
                var DataList = new List<FlowerModel>();
                foreach (DataRow dr in result.Rows)
                {
                    FlowerModel Obj = new FlowerModel();
                    Obj.FlowerID = Convert.ToInt32(dr["FlowerID"]);
                    Obj.FlowerName = Convert.ToString(dr["FlowerName"]);
                    Obj.FlowerCost = Convert.ToInt32(dr["FlowerCost"]);
                    Obj.FlowerFilename = Convert.ToString(dr["FlowerFileName"]);
                    Obj.FlowerFilePath = Convert.ToString(dr["FlowerFilePath"]);
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
        public async Task<ActionResult> LoadFlowerDetails(string FlowerID)
        {
            var result = FlowerAccessLayer.FetchFlowerDetailsById(FlowerID);
            if (result.Rows.Count > 0)
            {
                var DataList = new List<FlowerModel>();
                foreach (DataRow dr in result.Rows)
                {
                    FlowerModel Obj = new FlowerModel();
                    Obj.FlowerID = Convert.ToInt32(dr["FlowerID"]);
                    Obj.FlowerName = Convert.ToString(dr["FlowerName"]);
                    Obj.FlowerCost = Convert.ToInt32(dr["FlowerCost"]);
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

        public ActionResult LoadEditFlowerData(string FlowerID)
        {


            var result = FlowerAccessLayer.FetchFlowerDetailsById(FlowerID);
            if (result.Rows.Count > 0)
            {
                var DataList = new List<FlowerModel>();
                foreach (DataRow dr in result.Rows)
                {
                    FlowerModel Obj = new FlowerModel();
                    Obj.FlowerID = Convert.ToInt32(dr["FlowerID"]);
                    Obj.FlowerName = Convert.ToString(dr["FlowerName"]);
                    Obj.FlowerCost = Convert.ToInt32(dr["FlowerCost"]);
                    Obj.FlowerFilename = Convert.ToString(dr["FlowerFilename"]);
                    Obj.FlowerFilePath = Convert.ToString(dr["FlowerFilePath"]);
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
        public ActionResult DeleteFlowerData(string FlowerID, [FromServices] IWebHostEnvironment hostingEnvironment)
        {
            try
            {
                var dataTable = FlowerAccessLayer.FetchFlowerDetailsById(FlowerID);

                string wwwRootPath = hostingEnvironment.WebRootPath;

                string flowerImagesFolderPath = Path.Combine(wwwRootPath);

                if (dataTable != null && dataTable.Rows.Count > 0)
                {
                    // Access the FlowerFilePath from the first row of the DataTable
                    string flowerFileName = dataTable.Rows[0]["FlowerFilePath"].ToString().Replace("/", "\\");

                    // Combine the folder path with the file name
                    string flowerFilePath = Path.Combine(flowerImagesFolderPath, flowerFileName.TrimStart('\\'));

                    if (System.IO.File.Exists(flowerFilePath))
                    {
                        // Delete file from the filesystem
                        System.IO.File.Delete(flowerFilePath);

                        // Delete record from the SQL table
                        var result = FlowerAccessLayer.DeleteFlowerData(FlowerID);
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
                        return Json(new { IsSuccess = false, Message = "File not found for the specified FlowerID!" });
                    }
                }
                else
                {
                    return Json(new { IsSuccess = false, Message = "No record found for the specified FlowerID!" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { IsSuccess = false, Message = "Error occurred: " + ex.Message });
            }
        }



    }
}
