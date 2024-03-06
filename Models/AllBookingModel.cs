namespace Event.Models
{
    public class AllBookingModel
    {
        public List<BookingVenue> BookingVenue { get; set; }
        public List<EquipmentModel> BookingEquipment { get; set; }
        public List<FoodModel> BookingFood { get; set; }
        public List<FlowerModel> BookingFlower { get; set; }
        public List<LightModel> BookingLight { get; set; }
        public List<BookingDetailModel> BookingDetail {  get; set; }
    }
}
