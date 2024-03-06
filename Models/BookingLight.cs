using System.ComponentModel.DataAnnotations;

namespace Event.Models
{
    public class BookingLight
    {
        [Key]
        public int BookLightID { get; set; }
        public string LightType { get; set; }
        public int LightID { get; set; }
        public int BookingID { get; set; }
        public int Createdby { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
