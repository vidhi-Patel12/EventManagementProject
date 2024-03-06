using Event.Utility;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Event.Models.AccessLayer.AdminAccessLayer
{
    public class EquipmentAccessLayer
    {

        private static string connectionString = ConnectionString.Get("Connection");

        public static bool AddEquipment(string[] equipment, IFormFile file, string uploadDirectory,string  filePathInDatabase , HttpContext HttpContext)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("spAddEquipment", con);

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("EquipmentName", SqlDbType.VarChar).Value = equipment[1];
                    cmd.Parameters.Add("EquipmentCost", SqlDbType.Int).Value = Convert.ToInt32(equipment[2]);
                    cmd.Parameters.Add("@EquipmentFilename", SqlDbType.VarChar).Value = file.FileName;

                    string filePath = Path.Combine(uploadDirectory, file.FileName);
                    cmd.Parameters.Add("@EquipmentFilePath", SqlDbType.NVarChar).Value = filePathInDatabase;
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



        public static bool EditEquipment(string[] equipment, IFormFile file, string uploadDirectory,string filePathInDatabase ,HttpContext HttpContext)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("spUpdateEquipment", con);

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@EquipmentID", SqlDbType.Int).Value = Convert.ToInt32(equipment[0]);
                    cmd.Parameters.Add("@EquipmentName", SqlDbType.VarChar).Value = equipment[1];
                    cmd.Parameters.Add("@EquipmentCost", SqlDbType.Int).Value = Convert.ToInt32(equipment[2]);
                    cmd.Parameters.Add("@EquipmentFilename", SqlDbType.VarChar).Value = file.FileName;

                    string filePath = Path.Combine(uploadDirectory, file.FileName);
                    cmd.Parameters.Add("@EquipmentFilePath", SqlDbType.VarChar).Value = filePathInDatabase;
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


        public static DataTable FetchEquipmentUsingId(string Id)
        {
            DataTable dt = new();
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {

                    SqlCommand cmd = new SqlCommand("select * from Equipment where EquipmentID=@EquipmentID", con);
                    con.Open();
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add("EquipmentID", SqlDbType.Int).Value = Convert.ToInt32(Id);
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


        public static DataTable FetchEquipmentDetailsById(string EquipmentID)
        {
            DataTable dt = new();
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {

                    SqlCommand cmd = new SqlCommand("select * from Equipment where EquipmentID=@EquipmentID", con);
                    con.Open();
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add("EquipmentID", SqlDbType.Int).Value = Convert.ToInt32(EquipmentID);
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

        public static DataTable fetchEquipment()
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("select * from Equipment", con);
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

        public static bool DeleteEquipmentData(string DeleteData)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {

                    SqlCommand cmd = new SqlCommand("DELETE FROM Equipment\r\nWHERE  EquipmentID = @EquipmentID", con);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add("@EquipmentID", SqlDbType.Int).Value = Convert.ToString(DeleteData);

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
