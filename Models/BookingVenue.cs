using System.ComponentModel.DataAnnotations;

namespace Event.Models
{
    public class BookingVenue
    {
        [Key]
        public int BookingVenueID { get; set; }

        [Required(ErrorMessage = "Select Venue")]
        [Display(Description = "Venue Type")]
        public int VenueID { get; set; }

        [Required(ErrorMessage = "Select Event")]
        [Display(Description = " Event Type")]
        public int EventTypeID { get; set; }

        [Required(ErrorMessage = "Required Guest Count")]
        [Display(Description = "No .Of Guest")]
        public int GuestNo { get; set; }

        public int Createdby { get; set; }

        public DateTime? BookingDate { get; set; }

        public int BookingID { get; set; }

        public string VenueFilePath { get; set; }
    }
}
