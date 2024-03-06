using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace Event.Models
{
    public class RegistrationModel
    {
        [Key]
        public int ID { get; set; }
      
        [Required(ErrorMessage = "Name Required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Address Required")]
        public string Address { get; set; }
        [Required(ErrorMessage = "Country Required")]
        [CountryValidation]
        public string Country { get; set; }

       

        [StateValidation]
        [Required(ErrorMessage = "State Required")]
        public string State { get; set; }

        [CityValidation]
        [Required(ErrorMessage = "City Required")]
        public string City { get; set; }

        [Required(ErrorMessage = "Mobileno Required")]
        [RegularExpression(@"^(\d{10})$", ErrorMessage = "Wrong Mobileno")]
        public string Mobileno { get; set; }

        [Required(ErrorMessage = "EmailID Required")]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Please enter a valid e-mail adress")]
        public string EmailID { get; set; }

        [Required(ErrorMessage = "UserName Required")]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{6,}$", ErrorMessage = "Name must contain at least one uppercase letter, one number, one special character, and must be at least 6 characters long.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password Required")]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{6,}$", ErrorMessage = "Password must contain at least one uppercase letter, one number, one special character, and must be at least 6 characters long.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm Password Required")]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{6,}$", ErrorMessage = "Confirm Password must contain at least one uppercase letter, one number, one special character, and must be at least 6 characters long.")]
        [Compare("Password", ErrorMessage = "Enter Valid Confirm Password")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Gender Required")]
        public string Gender { get; set; }

        [Required(ErrorMessage = "BirthDate Required")]
        public DateTime? Birthdate { get; set; }
        [Required(ErrorMessage = "RoleID  Required")]
        public string RoleID { get; set; }

        public DateTime? CreatedOn { get; set; }

    }

    

    public class CountryValidation : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            string Message = string.Empty;
            if (Convert.ToString(value) == "0")
            {
                Message = "Choose Country";
                return new ValidationResult(Message);
            }
            else
            {
                return ValidationResult.Success;
            }
        }
    }

    public class StateValidation : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            string Message = string.Empty;
            if (Convert.ToString(value) == "0")
            {
                Message = "State Required";
                return new ValidationResult(Message);
            }
            else
            {
                return ValidationResult.Success;
            }
        }
    }

    public class CityValidation : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            string Message = string.Empty;
            if (Convert.ToString(value) == "0")
            {
                Message = "City Required";
                return new ValidationResult(Message);
            }
            else
            {
                return ValidationResult.Success;
            }
        }
    }
}

