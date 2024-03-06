using System.ComponentModel.DataAnnotations;

namespace Event.Models
{
    public class Dishtypes
    {
        [Key]
        public int ID { get; set; }
        public string Dishtype { get; set; }
    }
}
