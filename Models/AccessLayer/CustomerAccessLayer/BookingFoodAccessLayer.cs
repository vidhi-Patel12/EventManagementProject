using Event.Utility;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json.Linq;
using System.Data;

namespace Event.Models.AccessLayer.CustomerAccessLayer
{
    public class BookingFoodAccessLayer
    {

        private static string connectionString = ConnectionString.Get("Connection");
        public static DataTable GetAllFood()
        {
            DataTable dt = new();
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("select * from Food", con);
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

        public static bool BookingFood(string[] BookFoodlst, string[] Fooditem)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    foreach (var item in Fooditem)
                    {
                        SqlCommand cmd = new SqlCommand("INSERT INTO BookingFood(FoodType,MealType,DishType,FoodName,Createdby,CreatedDate,BookingID) VALUES ( @FoodType,@MealType,@DishType,@FoodName,@Createdby,@CreatedDate,@BookingID);", con);
                        cmd.CommandType = CommandType.Text;

                        cmd.Parameters.Add("@FoodType", SqlDbType.VarChar).Value = BookFoodlst[0];
                        cmd.Parameters.Add("@MealType", SqlDbType.VarChar).Value = BookFoodlst[1];
                        cmd.Parameters.Add("@DishType", SqlDbType.VarChar).Value = BookFoodlst[2];
                        cmd.Parameters.Add("@FoodName", SqlDbType.Int).Value = item;
                        cmd.Parameters.Add("@Createdby", SqlDbType.Int).Value = SessionClass.CreatedById;
                        cmd.Parameters.Add("@CreatedDate", SqlDbType.DateTime).Value = DateTime.Now;
                        cmd.Parameters.Add("@BookingID", SqlDbType.Int).Value = SessionClass.BookingId;
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
