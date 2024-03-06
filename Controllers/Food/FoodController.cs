﻿using Event.Models.AccessLayer.AdminAccessLayer;
using Event.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace Event.Controllers.Food
{
    public class FoodController : Controller
    {
        public ActionResult FoodIndex()
        {
            var Name = HttpContext.Session.GetString("Name");
            ViewBag.Name = Name;
            return View();
        }

        [HttpGet]
        public IActionResult AddFood()
        {
            return View();
        }
        [HttpPost]
        [HttpPost]
        public async Task<ActionResult> AddUpdateFood(IFormFile FoodFilepath, [FromServices] IWebHostEnvironment hostingEnvironment)
        {
            try
            {
                string[] foodlist = HttpContext.Request.Form["foodlist[]"];

                //var uploadDirectory = Path.Combine("wwwroot", "VenuImages");
                var uploadDirectory = Path.Combine(hostingEnvironment.WebRootPath, "FoodImages");

                Directory.CreateDirectory(uploadDirectory);

                if (FoodFilepath != null && FoodFilepath.Length > 0)
                {
                    var fileName = Path.GetFileName(FoodFilepath.FileName);
                    var filePathInDatabase = Path.Combine("/FoodImages", fileName);

                    if (!string.IsNullOrEmpty(foodlist[0]))
                    {
                        var result = FoodAccessLayer.FetchFoodUsingId(foodlist[0]);
                        if (result.Rows.Count > 0)
                        {
                            var resultUpdate = FoodAccessLayer.EditFood(foodlist, FoodFilepath, uploadDirectory, filePathInDatabase,HttpContext);
                            var filePath = Path.Combine(uploadDirectory, fileName);
                            using (var fileStream = new FileStream(filePath, FileMode.Create))
                            {
                                await FoodFilepath.CopyToAsync(fileStream);
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
                        var resultAdd = FoodAccessLayer.AddFood(foodlist, FoodFilepath, uploadDirectory, filePathInDatabase, HttpContext);
                        var filePath = Path.Combine(uploadDirectory, fileName);
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await FoodFilepath.CopyToAsync(fileStream);
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



        public ActionResult LoadFood()
        {
            var result = FoodAccessLayer.fetchfood();
            if (result.Rows.Count > 0)
            {
                var DataList = new List<FoodModel>();
                foreach (DataRow dr in result.Rows)
                {
                    FoodModel Obj = new FoodModel();
                    Obj.FoodID = Convert.ToInt32(dr["FoodID"]);
                    Obj.FoodName = Convert.ToString(dr["FoodName"]);
                    Obj.FoodCost = Convert.ToInt32(dr["FoodCost"]);
                    Obj.FoodFilename = Convert.ToString(dr["FoodFilename"]);
                    Obj.FoodFilePath = Convert.ToString(dr["FoodFilePath"]);
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
        public async Task<ActionResult> LoadFoodDetails(string FoodID)
        {
            var result = FoodAccessLayer.FetchFoodDetailsById(FoodID);
            if (result.Rows.Count > 0)
            {
                var DataList = new List<FoodModel>();
                foreach (DataRow dr in result.Rows)
                {
                    FoodModel Obj = new FoodModel();
                    Obj.FoodID = Convert.ToInt32(dr["FoodID"]);
                    Obj.FoodName = Convert.ToString(dr["FoodName"]);
                    Obj.FoodCost = Convert.ToInt32(dr["FoodCost"]);
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

        public ActionResult LoadEditFoodData(string FoodID)
        {
            var result = FoodAccessLayer.FetchFoodDetailsById(FoodID);
            if (result.Rows.Count > 0)
            {
                var DataList = new List<FoodModel>();
                foreach (DataRow dr in result.Rows)
                {
                    FoodModel Obj = new FoodModel();
                    Obj.FoodID = Convert.ToInt32(dr["FoodID"]);
                    Obj.FoodName = Convert.ToString(dr["FoodName"]);
                    Obj.FoodCost = Convert.ToInt32(dr["FoodCost"]);
                    Obj.FoodFilename = Convert.ToString(dr["FoodFilename"]);
                    Obj.FoodFilePath = Convert.ToString(dr["FoodFilePath"]);
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
        public ActionResult DeleteFoodData(string DeleteData)
        {
            try
            {
                if (DeleteData != "")
                {
                    var result = FoodAccessLayer.DeleteFoodData(DeleteData);
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