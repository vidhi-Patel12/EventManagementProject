using Event.Utility;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Event.Models.AccessLayer.CustomerAccessLayer
{
    public class ShowAllBookingAccessLayer
    {
        private static string connectionString = ConnectionString.Get("Connection");

        public static DataTable GetbookingdetailsbyBookingNo(string BookingNo)
        {
            DataTable dt = new();
            try
            {
                string query = "select * from BookingDetails where BookingNo =@BookingNo";
                using (SqlConnection con = new SqlConnection(connectionString))
                using (SqlCommand sqlCommand = new SqlCommand(query, con))
                {
                    con.Open();
                    sqlCommand.CommandType = CommandType.Text;
                    SqlDataAdapter da = new SqlDataAdapter(sqlCommand);
                    sqlCommand.Parameters.Add("@BookingNo", SqlDbType.VarChar).Value = BookingNo;
                    sqlCommand.Connection = con;
                    da.SelectCommand = sqlCommand;
                    con.Close();
                    da.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }
            return dt;
        }

        public static DataTable GetBookingDetails()
        {
            DataTable dt = new();
            try
            {
                string query = "select * from BookingDetails where Createdby =@Createdby";
                using (SqlConnection con = new SqlConnection(connectionString))
                using (SqlCommand sqlCommand = new SqlCommand(query, con))
                {
                    con.Open();
                    sqlCommand.CommandType = CommandType.Text;
                    SqlDataAdapter da = new SqlDataAdapter(sqlCommand);
                    sqlCommand.Parameters.Add("@Createdby", SqlDbType.VarChar).Value = SessionClass.CreatedById;
                    sqlCommand.Connection = con;
                    da.SelectCommand = sqlCommand;
                    con.Close();
                    da.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }
            return dt;
        }

        public static bool Cancel(string[] details)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();

                    using (SqlCommand cmd = new SqlCommand("UPDATE BookingDetails SET BookingNo=@BookingNo,BookingDate=@BookingDate,BookingApproval=@BookingApproval ,BookingApprovalDate=@BookingApprovalDate where BookingNo = @BookingNo;", con))
                    {
                        cmd.Connection = con;
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.Add("BookingNo", SqlDbType.VarChar).Value = Convert.ToString(details[0]);
                        cmd.Parameters.Add("BookingDate", SqlDbType.DateTime).Value = Convert.ToDateTime(details[1]);
                        cmd.Parameters.Add("BookingApproval", SqlDbType.Char).Value = "C";
                        cmd.Parameters.Add("BookingApprovalDate", SqlDbType.DateTime).Value = DateTime.Now;

                        cmd.ExecuteNonQuery();


                        con.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }


        public static DataTable GetVenuecost(string BookingNo)
        {
            DataTable dt = new DataTable();
            try
            {
                string query = "SELECT V.VenueName AS VenueName,V.VenueCost AS VenueCost FROM  Venue V INNER JOIN BookingVenue BV ON V.VenueID = BV.VenueID WHERE BV.BookingID = (SELECT BookingID FROM BookingDetails WHERE BookingNo=@BookingNo);";

                using (SqlConnection con = new SqlConnection(connectionString))
                using (SqlCommand sqlCommand = new SqlCommand(query, con))
                {
                    con.Open();
                    sqlCommand.CommandType = CommandType.Text;
                    sqlCommand.Parameters.Add("@BookingNo", SqlDbType.VarChar).Value = BookingNo;
                    sqlCommand.Connection = con;
                    SqlDataAdapter da = new SqlDataAdapter(sqlCommand);
                    da.SelectCommand = sqlCommand;
                    con.Close();
                    da.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return dt;
        }
        public static DataTable GetEquipmentcost(string BookingNo)
        {
            DataTable dt = new DataTable();
            try
            {
                string query = "SELECT E.EquipmentName AS EquipmentName,(E.EquipmentCost) AS EquipmentCost FROM Equipment E INNER JOIN BookingEquipment BE ON E.EquipmentID = BE.EquipmentID WHERE BE.BookingID =  (SELECT BookingID FROM BookingDetails WHERE BookingNo = @BookingNo)";

                using (SqlConnection con = new SqlConnection(connectionString))
                using (SqlCommand sqlCommand = new SqlCommand(query, con))
                {
                    con.Open();
                    sqlCommand.CommandType = CommandType.Text;
                    sqlCommand.Parameters.Add("@BookingNo", SqlDbType.VarChar).Value = Convert.ToString(BookingNo);
                    sqlCommand.Connection = con;
                    SqlDataAdapter da = new SqlDataAdapter(sqlCommand);
                    da.SelectCommand = sqlCommand;
                    con.Close();
                    da.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return dt;
        }
        public static DataTable GetFoodcost(string BookingNo)
        {
            DataTable dt = new DataTable();
            try
            {
                string query = "SELECT F.FoodName AS FoodName,F.FoodType AS FoodType,F.MealType AS MealType,(F.FoodCost) AS FoodCost FROM Food F INNER JOIN BookingFood BF ON F.FoodID = BF.FoodName WHERE BF.BookingID =  (SELECT BookingID FROM BookingDetails WHERE BookingNo = @BookingNo)";

                using (SqlConnection con = new SqlConnection(connectionString))
                using (SqlCommand sqlCommand = new SqlCommand(query, con))
                {
                    con.Open();
                    sqlCommand.CommandType = CommandType.Text;
                    sqlCommand.Parameters.Add("BookingNo", SqlDbType.VarChar).Value = Convert.ToString(BookingNo);
                    sqlCommand.Connection = con;
                    SqlDataAdapter da = new SqlDataAdapter(sqlCommand);
                    da.SelectCommand = sqlCommand;
                    con.Close();
                    da.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return dt;
        }
        public static DataTable GetLightCost(string BookingNo)
        {
            DataTable dt = new DataTable();
            try
            {
                string query = "SELECT  L.LightName AS LightName,L.LightType AS LightType,(L.LightCost) AS LightCost FROM Light L INNER JOIN BookingLight BL ON L.LightID = BL.LightID WHERE BL.BookingID =  (SELECT BookingID FROM BookingDetails WHERE BookingNo = @BookingNo)";

                using (SqlConnection con = new SqlConnection(connectionString))
                using (SqlCommand sqlCommand = new SqlCommand(query, con))
                {
                    con.Open();
                    sqlCommand.CommandType = CommandType.Text;
                    sqlCommand.Parameters.Add("BookingNo", SqlDbType.VarChar).Value = Convert.ToString(BookingNo);
                    sqlCommand.Connection = con;
                    SqlDataAdapter da = new SqlDataAdapter(sqlCommand);
                    da.SelectCommand = sqlCommand;
                    con.Close();
                    da.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return dt;
        }
        public static DataTable GetFlowerCost(string BookingNo)
        {
            DataTable dt = new DataTable();
            try
            {
                string query = "SELECT  F.FlowerName AS FlowerName,(F.FlowerCost) AS FlowerCost FROM Flower F INNER JOIN BookingFlower BF ON F.FlowerID = BF.FlowerID WHERE BF.BookingID =  (SELECT BookingID FROM BookingDetails WHERE BookingNo = @BookingNo)";

                using (SqlConnection con = new SqlConnection(connectionString))
                using (SqlCommand sqlCommand = new SqlCommand(query, con))
                {
                    con.Open();
                    sqlCommand.CommandType = CommandType.Text;
                    sqlCommand.Parameters.Add("BookingNo", SqlDbType.VarChar).Value = Convert.ToString(BookingNo);
                    sqlCommand.Connection = con;
                    SqlDataAdapter da = new SqlDataAdapter(sqlCommand);
                    da.SelectCommand = sqlCommand;
                    con.Close();
                    da.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return dt;
        }
        public static DataTable Getbookingdetails(string BookingNo)
        {
            DataTable dt = new DataTable();
            try
            {
                string query = "select Createdby,BookingNo,BookingDate From BookingDetails Where Createdby = @Createdby";
                using (SqlConnection con = new SqlConnection(connectionString))
                using (SqlCommand sqlCommand = new SqlCommand(query, con))
                {
                    con.Open();
                    sqlCommand.CommandType = CommandType.Text;
                    sqlCommand.Parameters.Add("@Createdby", SqlDbType.VarChar).Value = SessionClass.CreatedById;
                    sqlCommand.Connection = con;
                    SqlDataAdapter da = new SqlDataAdapter(sqlCommand);
                    da.SelectCommand = sqlCommand;
                    con.Close();
                    da.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return dt;
        }
    }
}
