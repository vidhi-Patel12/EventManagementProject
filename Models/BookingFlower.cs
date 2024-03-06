using System.ComponentModel.DataAnnotations;

namespace Event.Models
{
    public class BookingFlower
    {
        [Key]
        public int BookingFlowerID { get; set; }
        public int FlowerID { get; set; }
        public int Createdby { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int BookingID { get; set; }
    }
}
