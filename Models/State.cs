using System.ComponentModel.DataAnnotations;

namespace Event.Models
{
    public class State
    {

        [Key]
        public int StateID { get; set; }
        public string StateName { get; set; }
        public int CountryID { get; set; }
    }
}
