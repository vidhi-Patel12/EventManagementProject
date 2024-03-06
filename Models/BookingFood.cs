using System.ComponentModel.DataAnnotations;

namespace Event.Models
{
    public class BookingFood
    {
        [Key]
        public int BookFoodID { get; set; }

        [Required(ErrorMessage = "Select FoodType")]
        public string FoodType { get; set; }

        [Required(ErrorMessage = "Select MealType")]
        public string MealType { get; set; }

        [Required(ErrorMessage = "Select DishType")]
        public string DishType { get; set; }

        public int FoodName { get; set; }

        public int Createdby { get; set; }
        public DateTime CreatedDate { get; set; }
        public int BookingID { get; set; }
    }
}
