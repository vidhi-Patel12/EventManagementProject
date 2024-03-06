using System.ComponentModel.DataAnnotations;

namespace Event.Models
{
    public class Eventclass
    {
        [Key]
        public int EventID { get; set; }
        public string EventType { get; set; }
        public string Status { get; set; }
    }
}
