using Event.Models.AccessLayer.AdminAccessLayer;
using Event.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace Event.Controllers.Light
{
    public class LightController : Controller
    {
        public ActionResult LightIndex()
        {
            var Name = HttpContext.Session.GetString("Name");
            ViewBag.Name = Name;
            return View();
        }

        [HttpGet]
        public IActionResult AddLight()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> AddUpdateLight(IFormFile LightFilepath, [FromServices] IWebHostEnvironment hostingEnvironment)
        {
            try
            {
                string[] lightlist = HttpContext.Request.Form["lightlist[]"];

                //var uploadDirectory = Path.Combine("wwwroot", "VenuImages");
                var uploadDirectory = Path.Combine(hostingEnvironment.WebRootPath, "LightImages");

                Directory.CreateDirectory(uploadDirectory);

                if (LightFilepath != null && LightFilepath.Length > 0)
                {
                    var fileName = Path.GetFileName(LightFilepath.FileName);
                    var filePathInDatabase = Path.Combine("/LightImages", fileName);

                    if (!string.IsNullOrEmpty(lightlist[0]))
                    {
                        var result = LightAccessLayer.FetchLightUsingId(lightlist[0]);
                        if (result.Rows.Count > 0)
                        {
                            var resultUpdate = LightAccessLayer.EditLight(lightlist, LightFilepath, uploadDirectory, filePathInDatabase, HttpContext);
                            var filePath = Path.Combine(uploadDirectory, fileName);
                            using (var fileStream = new FileStream(filePath, FileMode.Create))
                            {
                                await LightFilepath.CopyToAsync(fileStream);
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
                        var resultAdd = LightAccessLayer.AddLight(lightlist, LightFilepath, uploadDirectory, filePathInDatabase, HttpContext);
                        var filePath = Path.Combine(uploadDirectory, fileName);
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await LightFilepath.CopyToAsync(fileStream);
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

        public ActionResult LoadLight()
        {
            var result = LightAccessLayer.fetchlight();
            if (result.Rows.Count > 0)
            {
                var DataList = new List<LightModel>();
                foreach (DataRow dr in result.Rows)
                {
                    LightModel Obj = new LightModel();
                    Obj.LightID = Convert.ToInt32(dr["LightID"]);
                    Obj.LightName = Convert.ToString(dr["LightName"]);
                    Obj.LightCost = Convert.ToInt32(dr["LightCost"]);
                    Obj.LightFilename = Convert.ToString(dr["LightFilename"]);
                    Obj.LightFilePath = Convert.ToString(dr["LightFilePath"]);
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
        public async Task<ActionResult> LoadLightDetails(string LightID)
        {
            var result = LightAccessLayer.FetchLightDetailsById(LightID);
            if (result.Rows.Count > 0)
            {
                var DataList = new List<LightModel>();
                foreach (DataRow dr in result.Rows)
                {
                    LightModel Obj = new LightModel();
                    Obj.LightID = Convert.ToInt32(dr["LightID"]);
                    Obj.LightName = Convert.ToString(dr["LightName"]);
                    Obj.LightCost = Convert.ToInt32(dr["LightCost"]);
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

        public ActionResult LoadEditLightData(string LightID)
        {
            var result = LightAccessLayer.FetchLightDetailsById(LightID);
            if (result.Rows.Count > 0)
            {
                var DataList = new List<LightModel>();
                foreach (DataRow dr in result.Rows)
                {
                    LightModel Obj = new LightModel();
                    Obj.LightID = Convert.ToInt32(dr["LightID"]);
                    Obj.LightName = Convert.ToString(dr["LightName"]);
                    Obj.LightCost = Convert.ToInt32(dr["LightCost"]);
                    Obj.LightFilename = Convert.ToString(dr["LightFilename"]);
                    Obj.LightFilePath = Convert.ToString(dr["LightFilePath"]);
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
        public ActionResult DeleteLightData(string DeleteData)
        {
            try
            {
                if (DeleteData != "")
                {
                    var result = LightAccessLayer.DeleteLightData(DeleteData);
                    if (result)
                    {
                        return Json(new { IsSuccess = true, Message = "Record is deleted!" });
                    }
                    else
                    {
                        return Json(new { IsSuccess = false, Message = "Something went wrong while deleting record!" });
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(new { IsSuccess = false, Message = "Something went wrong, Please try again later!" });
        }


    }
}