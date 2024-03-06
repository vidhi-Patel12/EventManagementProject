using Event.Utility;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Event.Models.AccessLayer.CustomerAccessLayer
{
    public class BookingEquipmentAccessLayer
    {
        private static string connectionString = ConnectionString.Get("Connection");
        public static DataTable BookingID()
        {
            DataTable dt = new();
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {

                    SqlCommand cmd = new SqlCommand("select BookingID , Createdby from BookingDetails  ", con);
                    con.Open();
                    cmd.CommandType = CommandType.Text;

                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = cmd;
                    da.Fill(dt);
                    cmd.ExecuteNonQuery();
                    con.Close();


                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }


        public static DataTable GetAllEquipment()
        {
            DataTable dt = new();
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("select * from Equipment", con);
                    con.Open();
                    cmd.CommandType = CommandType.Text;
                    //cmd.Parameters.Add("CountryID", SqlDbType.Int).Value = Convert.ToInt32(CountryID);
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

        public static bool BookingEquipment(string[] BookEquipmentlst)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    foreach (var item in BookEquipmentlst)
                    {
                        SqlCommand cmd = new SqlCommand("INSERT INTO BookingEquipment(EquipmentID,Createdby,CreatedDate,BookingID) VALUES (@EquipmentID,@Createdby,@CreatedDate,@BookingID);", con);
                        cmd.CommandType = CommandType.Text;

                        cmd.Parameters.Add("@EquipmentID", SqlDbType.Int).Value = item;
                        cmd.Parameters.Add("@Createdby", SqlDbType.Int).Value = SessionClass.CreatedById;
                        cmd.Parameters.Add("@CreatedDate", SqlDbType.DateTime).Value = DateTime.Now;
                        cmd.Parameters.Add("@BookingID", SqlDbType.Int).Value = SessionClass.BookingId;
                        //cmd.Parameters.Add("@EquipmentFilePath", SqlDbType.NVarChar).Value = Convert.ToString(BookEquipmentlst[2]);
                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

    }
}
