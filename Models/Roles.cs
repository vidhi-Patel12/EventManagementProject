using System.ComponentModel.DataAnnotations;
using System.Data.SqlTypes;

namespace Event.Models
{
    public class Roles
    {
        [Key]
        public string RoleID { get; set; }
        public string Rolename { get; set; }
    }
}
