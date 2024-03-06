using Event.Utility;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Event.Models.AccessLayer.AdminAccessLayer
{
    public class LightAccessLayer
    {

        private static string connectionString = ConnectionString.Get("Connection");


        public static bool AddLight(string[] light, IFormFile file, string uploadDirectory,String filePathInDatabase, HttpContext HttpContext)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("spAddLight", con);

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@LightType", SqlDbType.VarChar).Value = light[1];
                    cmd.Parameters.Add("@LightName", SqlDbType.VarChar).Value = light[2];
                    cmd.Parameters.Add("@LightCost", SqlDbType.Int).Value = Convert.ToInt32(light[3]);
                    cmd.Parameters.Add("@LightFilename", SqlDbType.VarChar).Value = file.FileName;

                    string filePath = Path.Combine(uploadDirectory, file.FileName);
                    cmd.Parameters.Add("@LightFilePath", SqlDbType.NVarChar).Value = filePathInDatabase;
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



        public static bool EditLight(string[] light, IFormFile file, string uploadDirectory,string filePathInDatabase, HttpContext HttpContext)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("spUpdateLight", con);

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@LightID", SqlDbType.Int).Value = Convert.ToInt32(light[0]);
                    cmd.Parameters.Add("@LightType", SqlDbType.Char).Value = light[1];
                    cmd.Parameters.Add("@LightName", SqlDbType.VarChar).Value = light[2];
                    cmd.Parameters.Add("@LightCost", SqlDbType.Int).Value = Convert.ToInt32(light[3]);
                    cmd.Parameters.Add("@LightFilename", SqlDbType.VarChar).Value = file.FileName;

                    string filePath = Path.Combine(uploadDirectory, file.FileName);
                    cmd.Parameters.Add("@LightFilePath", SqlDbType.VarChar).Value = filePathInDatabase;
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


        public static DataTable FetchLightUsingId(string Id)
        {
            DataTable dt = new();
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {

                    SqlCommand cmd = new SqlCommand("select * from Light where LightID=@LightID", con);
                    con.Open();
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add("LightID", SqlDbType.Int).Value = Convert.ToInt32(Id);
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


        public static DataTable FetchLightDetailsById(string LightID)
        {
            DataTable dt = new();
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {

                    SqlCommand cmd = new SqlCommand("select * from Light where LightID=@LightID", con);
                    con.Open();
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add("LightID", SqlDbType.Int).Value = Convert.ToInt32(LightID);
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

        public static DataTable fetchlight()
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("select * from Light", con);
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

        public static bool DeleteLightData(string DeleteData)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {

                    SqlCommand cmd = new SqlCommand("DELETE FROM Light\r\nWHERE  LightID = @LightID", con);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add("@LightID", SqlDbType.Int).Value = Convert.ToString(DeleteData);

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
