using System.ComponentModel.DataAnnotations;

namespace Event.Models
{
    public class LightModel
    {
        [Key]
        public int LightID { get; set; }

        [Required(ErrorMessage = "Required Light Type")]
        public string LightType { get; set; }

        [Required(ErrorMessage = "Required Light Name")]
        public string LightName { get; set; }

        public string LightFilename { get; set; }

        public string LightFilePath { get; set; }

        public int? Createdby { get; set; }
        public DateTime? Createdate { get; set; }

        [RegularExpression("([1-9][0-9]*)", ErrorMessage = "Enter only numeric number")]
        [Required(ErrorMessage = "Required Light Cost")]
        public int LightCost { get; set; }

    }
}
