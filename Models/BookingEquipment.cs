using System.ComponentModel.DataAnnotations;

namespace Event.Models
{
    public class BookingEquipment
    {
        [Key]
        public int BookingEquipmentID { get; set; }

        public int EquipmentID { get; set; }

        public int Createdby { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int BookingID { get; set; }
    }
}
