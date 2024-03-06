using System.ComponentModel.DataAnnotations;

namespace Event.Models
{
    public class VenueModel
    {
        [Key]
        public int VenueID { get; set; }

        [Required(ErrorMessage = "VenueName Required")]
        public string VenueName { get; set; }

        public string VenueFilename { get; set; }

        [Required(ErrorMessage = "VenueFile Required")]
        public string VenueFilePath { get; set; }

        public int Createdby { get; set; }

        public DateTime? Createdate { get; set; }

        [Required(ErrorMessage = "VenueCost Required")]
        [RegularExpression("([1-9][0-9]*)", ErrorMessage = "Enter only numeric number")]
        public int? VenueCost { get; set; }
    }
}
