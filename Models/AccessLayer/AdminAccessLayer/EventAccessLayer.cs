using Event.Utility;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;

namespace Event.Models
{
    public class EventAccessLayer
    {
        //public static string connectionString = ConnectionString.Get();

        private static string connectionString = ConnectionString.Get("Connection");

       


        public static DataTable GetAllCountry()
        {
            DataTable dt = new();
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("GetAllCountry", con);
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
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

        public static DataTable GetState(string CountryID)
        {
            DataTable dt = new();
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("GetAllState", con);
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("CountryID", SqlDbType.Int).Value = Convert.ToInt32(CountryID);
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

        public static  DataTable GetCity(string StateID)
        {
            DataTable dt = new();
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("GetAllCity", con);
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                   cmd.Parameters.Add("StateID", SqlDbType.Int).Value = Convert.ToInt32(StateID);
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



        public static bool Registration(string[] Register)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();

                    // Check if the username already exists
                    SqlCommand checkCmd = new SqlCommand("SELECT COUNT(*) FROM Registration WHERE Username = @Username", con);
                    checkCmd.Parameters.AddWithValue("@Username", Register[7]);
                    int count = (int)checkCmd.ExecuteScalar();

                    if (count > 0)
                    {
                        // Username already exists, return false
                        return false;
                    }

                    // Username is unique, proceed with registration
                    SqlCommand cmd = new SqlCommand("INSERT INTO Registration (Name, Address, City, State, Country, Mobileno, EmailId, Username, Password, ConfirmPassword, Gender, Birthdate, RoleID, CreatedOn) VALUES (@Name, @Address, @City, @State, @Country, @Mobileno, @EmailId, @Username, @Password, @ConfirmPassword, @Gender, @Birthdate, @RoleID, @CreatedOn);", con);
                    cmd.CommandType = CommandType.Text;

                    // Add parameters
                    cmd.Parameters.AddWithValue("@Name", Register[0]);
                    cmd.Parameters.AddWithValue("@Address", Register[1]);
                    cmd.Parameters.AddWithValue("@Country", Register[2]);
                    cmd.Parameters.AddWithValue("@State", Register[3]);
                    cmd.Parameters.AddWithValue("@City", Register[4]);
                    cmd.Parameters.AddWithValue("@Mobileno", Register[5]);
                    cmd.Parameters.AddWithValue("@EmailId", Register[6]);
                    cmd.Parameters.AddWithValue("@Username", Register[7]);
                    cmd.Parameters.AddWithValue("@Password", Register[8]);
                    cmd.Parameters.AddWithValue("@ConfirmPassword", Register[9]);
                    cmd.Parameters.AddWithValue("@Gender", Register[10]);
                    cmd.Parameters.AddWithValue("@Birthdate", Register[11]);
                    cmd.Parameters.AddWithValue("@RoleId", (Register[12]));
                    cmd.Parameters.AddWithValue("@CreatedOn", DateTime.Now);

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


        public static DataTable FetchUserProfileDetailsById(string RegistrationID)
        {
            DataTable dt = new();
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {

                    SqlCommand cmd = new SqlCommand("select * from Registration where ID=@ID", con);
                    con.Open();
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add("ID", SqlDbType.Int).Value = Convert.ToInt32(RegistrationID);
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

        public static DataTable fetchUserProfile()
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("SELECT  r.Name, r.Username, r.Mobileno,r.Gender,r.EmailID, r.Birthdate, r.Address,r.CreatedOn,cr.Name AS Country,s.StateName AS State,cy.CityName AS City FROM  Registration r LEFT JOIN  Country cr ON r.Country = cr.Name LEFT JOIN   States s ON cr.CountryID = s.CountryID LEFT JOIN City cy ON s.StateID = cy.StateID where r.Name = @Name;", con);
                    con.Open();
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add("@Name", SqlDbType.VarChar).Value = SessionClass.Name;
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




        public static DataTable  Login(string[] login)
        {
            DataTable dt = new();
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {

                    SqlCommand cmd = new SqlCommand("select ID, Name,Username , Password ,RoleID from Registration where Username=@Username AND Password=@Password", con);
                    con.Open();
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add("@Username", SqlDbType.VarChar).Value = login[0];
                    cmd.Parameters.Add("@Password", SqlDbType.VarChar).Value = login[1];
                    //cmd.Parameters.Add("@RoleID", SqlDbType.Int).Value = Convert.ToInt32(RoleID);
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



        public static bool AddVenue(string[] venuelist, IFormFile file, string uploadDirectory, string filePathInDatabase, HttpContext HttpContext)
        {
            try
            {

                using (SqlConnection con = new SqlConnection(connectionString))
                {

                    //SqlCommand cmd = new SqlCommand("INSERT INTO Venue (VenueName, VenueCost,VenueFilename,VenueFilePath)\r\nVALUES (@VenueName, @VenueCost,@VenueFilename,@VenueFilePath);", con);
                    SqlCommand cmd = new SqlCommand("spAddVenue", con);


                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@VenueName", SqlDbType.VarChar).Value = venuelist[1];
                    cmd.Parameters.Add("@VenueCost", SqlDbType.Int).Value = Convert.ToInt32(venuelist[2]);
                    cmd.Parameters.Add("@VenueFilename", SqlDbType.VarChar).Value = file.FileName;

                    string filePath = Path.Combine(uploadDirectory, file.FileName);
                    cmd.Parameters.Add("VenueFilePath", SqlDbType.NVarChar).Value = filePathInDatabase;
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



        public static bool EditVenue(string[] venuelist, IFormFile file, string uploadDirectory, string filePathInDatabase)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("spUpdateVenue", con);

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@VenueID", SqlDbType.Int).Value = Convert.ToInt32(venuelist[0]);
                    cmd.Parameters.Add("@VenueName", SqlDbType.VarChar).Value = venuelist[1];
                    cmd.Parameters.Add("@VenueCost", SqlDbType.Int).Value = Convert.ToInt32(venuelist[2]);
                    cmd.Parameters.Add("@VenueFilename", SqlDbType.VarChar).Value = file.FileName;

                    string filePath = Path.Combine(uploadDirectory, file.FileName);
                    cmd.Parameters.Add("VenueFilepath", SqlDbType.NVarChar).Value = filePathInDatabase;

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


        public static DataTable FetchVenueUsingId(string Id)
        {
            DataTable dt = new();
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {

                    SqlCommand cmd = new SqlCommand("select * from Venue where VenueID=@VenueID", con);
                    con.Open();
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add("VenueID", SqlDbType.Int).Value = Convert.ToInt32(Id);
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


        public static DataTable FetchVenueDetailsById(string VenueID)
        {
            DataTable dt = new();
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {

                    SqlCommand cmd = new SqlCommand("select * from Venue where VenueID=@VenueID", con);
                    con.Open();
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add("VenueID", SqlDbType.Int).Value = Convert.ToInt32(VenueID);
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

        public static DataTable fetchVenue()
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("select * from Venue", con);
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

        public static bool DeleteVenueData(string DeleteData)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {

                    SqlCommand cmd = new SqlCommand("DELETE FROM Venue\r\nWHERE  VenueID = @VenueID", con);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add("@VenueID", SqlDbType.Int).Value = Convert.ToString(DeleteData);

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











                              