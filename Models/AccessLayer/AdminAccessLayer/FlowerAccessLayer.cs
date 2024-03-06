using Event.Utility;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Event.Models.AccessLayer.AdminAccessLayer
{
    public class FlowerAccessLayer
    {

        private static string connectionString = ConnectionString.Get("Connection");

        public static bool AddFlower(string[] Flowerlist, IFormFile file, string uploadDirectory,string filePathInDatabase, HttpContext HttpContext)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    //SqlCommand cmd = new SqlCommand("INSERT INTO Flower (FlowerName, FlowerCost,FlowerFilename,FlowerFilePath)\r\nVALUES (@FlowerName, @FlowerCost,@FlowerFilename,@FlowerFilePath);", con);
                    SqlCommand cmd = new SqlCommand("spAddFlower", con);

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@FlowerName", SqlDbType.VarChar).Value = Flowerlist[1];
                    cmd.Parameters.Add("@FlowerCost", SqlDbType.Int).Value = Convert.ToInt32(Flowerlist[2]);
                    cmd.Parameters.Add("@FlowerFilename", SqlDbType.VarChar).Value = file.FileName;

                    string filePath = Path.Combine(uploadDirectory, file.FileName);
                    cmd.Parameters.Add("@FlowerFilePath", SqlDbType.NVarChar).Value = filePathInDatabase;
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



        public static bool EditFlower(string[] Flowerlist, IFormFile file, string uploadDirectory,string filePathInDatabase, HttpContext HttpContext)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("spUpdateFlower", con);

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@FlowerID", SqlDbType.Int).Value = Convert.ToInt32(Flowerlist[0]);
                    cmd.Parameters.Add("@FlowerName", SqlDbType.VarChar).Value = Flowerlist[1];
                    cmd.Parameters.Add("@FlowerCost", SqlDbType.Int).Value = Convert.ToInt32(Flowerlist[2]);
                    cmd.Parameters.Add("@FlowerFilename", SqlDbType.VarChar).Value = file.FileName;

                    string filePath = Path.Combine(uploadDirectory, file.FileName);
                    cmd.Parameters.Add("@FlowerFilePath", SqlDbType.VarChar).Value = filePath;
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


        public static DataTable FetchFlowerUsingId(string Id)
        {
            DataTable dt = new();
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {

                    SqlCommand cmd = new SqlCommand("select * from Flower where FlowerID=@FlowerID", con);
                    con.Open();
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add("FlowerID", SqlDbType.Int).Value = Convert.ToInt32(Id);
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


        public static DataTable FetchFlowerDetailsById(string FlowerID)
        {
            DataTable dt = new();
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {

                    SqlCommand cmd = new SqlCommand("select * from Flower where FlowerID=@FlowerID", con);
                    con.Open();
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add("FlowerID", SqlDbType.Int).Value = Convert.ToInt32(FlowerID);
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

        public static DataTable fetchFlower()
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("select * from Flower", con);
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

        public static bool DeleteFlowerData(string DeleteData)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {

                    SqlCommand cmd = new SqlCommand("DELETE FROM Flower\r\nWHERE  FlowerID = @FlowerID", con);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add("@FlowerID", SqlDbType.Int).Value = Convert.ToString(DeleteData);

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





