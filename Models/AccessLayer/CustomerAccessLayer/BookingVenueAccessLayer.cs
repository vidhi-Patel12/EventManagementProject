using Event.Utility;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Event.Models.AccessLayer.CustomerAccessLayer
{
    public class BookingVenueAccessLayer
    {
        private static string connectionString = ConnectionString.Get("Connection");

        public static DataTable GetAllEvent()
        {
            DataTable dt = new();
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("select * from Event", con);
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




        public static DataTable GetAllVenue()
        {
            DataTable dt = new();
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("select VenueID,VenueName, VenueFilePath from Venue", con);
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



        public static DataTable BookingNousingID(int bookingno)
        {
            DataTable dt = new();
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {

                    SqlCommand cmd = new SqlCommand("UPDATE BookingDetails SET BookingNo=@BookingNo where BookingID = @BookingID;", con);
                    con.Open();
                    cmd.CommandType = CommandType.Text;

                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = cmd;
                    cmd.Parameters.Add("@BookingID", SqlDbType.Int).Value = Convert.ToInt32(bookingno); // Assuming
                    cmd.Parameters.Add("@BookingNo", SqlDbType.VarChar).Value ="BK-"+Convert.ToString(bookingno);
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

        public static DataTable BookingDetailsID()
        {
            DataTable dt = new();
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {

                    SqlCommand cmd = new SqlCommand("select BookingID from BookingDetails  ", con);
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

        public static bool BookingDetails(string[] BookVenuelst)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    
                    SqlCommand cmd = new SqlCommand("INSERT INTO BookingDetails(BookingDate, Createdby,CreatedDate,BookingApproval) VALUES (@BookingDate, @Createdby,@CreatedDate,@BookingApproval);", con);
                    cmd.CommandType = CommandType.Text;

                    //cmd.Parameters.Add("@BookingNo", SqlDbType.VarChar).Value ="BK"+"-"+ Convert.ToString(BookVenuelst[0]);
                    cmd.Parameters.Add("@BookingDate", SqlDbType.Date).Value = Convert.ToDateTime(BookVenuelst[0]);
                    cmd.Parameters.Add("@Createdby", SqlDbType.Int).Value = SessionClass.CreatedById;
                    cmd.Parameters.Add("@CreatedDate", SqlDbType.DateTime).Value = DateTime.Now;
                    cmd.Parameters.Add("@BookingApproval", SqlDbType.Char).Value = "P";
                   
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }


        public static bool BookingVenue(string[] BookVenuelst)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    
                    SqlCommand cmd = new SqlCommand("INSERT INTO BookingVenue(EventID,VenueID, GuestNo,Createdby,BookingDate,BookingID) VALUES ( @EventID,@VenueID, @GuestNo,@Createdby,@BookingDate,@BookingID);", con);
                    cmd.CommandType = CommandType.Text;

                    cmd.Parameters.Add("@EventID", SqlDbType.Int).Value = Convert.ToInt32(BookVenuelst[0]);
                    cmd.Parameters.Add("@VenueID", SqlDbType.Int).Value = Convert.ToInt32(BookVenuelst[1]);
                    cmd.Parameters.Add("@GuestNo", SqlDbType.Int).Value = Convert.ToInt32(BookVenuelst[2]);
                    cmd.Parameters.Add("@Createdby", SqlDbType.Int).Value = SessionClass.CreatedById;
                    cmd.Parameters.Add("@BookingDate", SqlDbType.Date).Value = Convert.ToDateTime(BookVenuelst[3]);
                    cmd.Parameters.Add("@BookingID", SqlDbType.Int).Value = SessionClass.BookingId;
                    //string filePath = Path.Combine(uploadDirectory, file.FileName);
                   // cmd.Parameters.Add("@VenueFilePath", SqlDbType.NVarChar).Value = Convert.ToString(BookVenuelst[4]);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }


        public static string IsVenueBookedOnDate(DateTime bookingDate, int venueID)
        {
            string message = "";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    string query = "SELECT COUNT(*) FROM BookingVenue WHERE BookingDate = @BookingDate AND VenueID = @VenueID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@BookingDate", bookingDate);
                        command.Parameters.AddWithValue("@VenueID", venueID);

                        connection.Open();

                        int count = (int)command.ExecuteScalar();

                        if (count > 0)
                        {
                            message = "Date is already booked for the same venue.";
                        }
                        else
                        {
                            // Check if the same date is booked for a different venue
                            query = "SELECT COUNT(*) FROM BookingVenue WHERE BookingDate = @BookingDate AND VenueID != @VenueID";
                            command.CommandText = query;

                            int differentVenueCount = (int)command.ExecuteScalar();

                            if (differentVenueCount > 0)
                            {
                                message = "Date is booked for a different venue.";
                            }
                            else
                            {
                                message = "Booking is successful.";
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Handle exception
                    Console.WriteLine(ex.Message);
                    // Log the exception
                }
            }

            return message;
        }




    }
}
