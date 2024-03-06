using System.ComponentModel.DataAnnotations;

namespace Event.Models
{
    public class BookingDetailModel
    {
        [Key]
        public int BookingID { get; set; }
        public string BookingNo { get; set; }
        public DateTime? BookingDate { get; set; }
        public int Createdby { get; set; }
        public DateTime CreatedDate { get; set; }
        public string BookingApproval { get; set; }
        public DateTime? BookingApprovalDate { get; set; }
        public bool BookingCompletedFlag { get; set; }
        public int VenueCost { get; internal set; }
        public int EquipmentCost { get; internal set; }
        public int FoodCost { get; internal set; }
        public int LightCost { get; internal set; }
        public int FlowerCost { get; internal set; }
    }
}
