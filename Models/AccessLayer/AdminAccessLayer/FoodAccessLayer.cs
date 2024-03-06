using Event.Utility;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Event.Models.AccessLayer.AdminAccessLayer
{
    public class FoodAccessLayer
    {
        private static string connectionString = ConnectionString.Get("Connection");

        public static bool AddFood(string[] food, IFormFile file, string uploadDirectory, string filePathInDatabase, HttpContext HttpContext)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("spAddFood", con);

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@FoodType", SqlDbType.VarChar).Value = food[1];
                    cmd.Parameters.Add("@MealType", SqlDbType.VarChar).Value = food[2];
                    cmd.Parameters.Add("@DishType", SqlDbType.VarChar).Value = food[3];
                    cmd.Parameters.Add("@FoodName", SqlDbType.VarChar).Value = food[4];
                    cmd.Parameters.Add("@FoodCost", SqlDbType.Int).Value = Convert.ToInt32(food[5]);
                    cmd.Parameters.Add("@FoodFilename", SqlDbType.VarChar).Value = file.FileName;

                    string filePath = Path.Combine(uploadDirectory, file.FileName);
                    cmd.Parameters.Add("@FoodFilePath", SqlDbType.NVarChar).Value = filePathInDatabase;
                    cmd.Parameters.Add("@Createdby", SqlDbType.Int).Value = HttpContext.Session.GetInt32("UserId");
                    cmd.Parameters.Add("@Createdate", SqlDbType.DateTime).Value = DateTime.Now;

                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }



        public static bool EditFood(string[] food, IFormFile file, string uploadDirectory, string filePathInDatabase,HttpContext HttpContext)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("spUpdateFood", con);

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@FoodID", SqlDbType.Int).Value = Convert.ToInt32(food[0]);
                    cmd.Parameters.Add("@FoodType", SqlDbType.VarChar).Value = food[1];
                    cmd.Parameters.Add("@MealType", SqlDbType.VarChar).Value = food[2];
                   
                    cmd.Parameters.Add("@DishType", SqlDbType.VarChar).Value = food[3];
                    cmd.Parameters.Add("@FoodName", SqlDbType.VarChar).Value = food[4];
                    cmd.Parameters.Add("@FoodCost", SqlDbType.Int).Value = Convert.ToInt32(food[5]);
                    cmd.Parameters.Add("@FoodFilename", SqlDbType.VarChar).Value = file.FileName;

                    string filePath = Path.Combine(uploadDirectory, file.FileName);
                    cmd.Parameters.Add("@FoodFilePath", SqlDbType.VarChar).Value = filePathInDatabase;
                    cmd.Parameters.Add("@Createdby", SqlDbType.Int).Value = HttpContext.Session.GetInt32("UserId");
                    cmd.Parameters.Add("@Createdate", SqlDbType.DateTime).Value = DateTime.Now;

                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }


        public static DataTable FetchFoodUsingId(string Id)
        {
            DataTable dt = new();
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {

                    SqlCommand cmd = new SqlCommand("select * from Food where FoodID=@FoodID", con);
                    con.Open();
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add("FoodID", SqlDbType.Int).Value = Convert.ToInt32(Id);
                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = cmd;
                    da.Fill(dt);
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }


        public static DataTable FetchFoodDetailsById(string FoodID)
        {
            DataTable dt = new();
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {

                    SqlCommand cmd = new SqlCommand("select * from Food where FoodID=@FoodID", con);
                    con.Open();
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add("FoodID", SqlDbType.Int).Value = Convert.ToInt32(FoodID);
                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = cmd;
                    da.Fill(dt);
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }

        public static DataTable fetchfood()
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("select * from Food", con);
                    con.Open();
                    cmd.CommandType = CommandType.Text;
                    // cmd.Parameters.Add("IsActive", SqlDbType.Bit).Value = false;
                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = cmd;
                    da.Fill(dt);
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }

        public static bool DeleteFoodData(string DeleteData)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {

                    SqlCommand cmd = new SqlCommand("DELETE FROM Food\r\nWHERE  FoodID = @FoodID", con);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add("@FoodID", SqlDbType.Int).Value = Convert.ToString(DeleteData);

                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

    }
}
