using System.ComponentModel.DataAnnotations;

namespace Event.Models
{
    public class Country
    {
        [Key]
        public int CountryID { get; set; }
        public string Name { get; set; }
    }
}
