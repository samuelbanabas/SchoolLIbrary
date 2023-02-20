using System.ComponentModel.DataAnnotations;

namespace SchoolLIbrary.Models.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "The student registration number is required")]
        [Display(Name = "Student RegNo")]
        public string RegNo { get; set; }

        [Required(ErrorMessage = "The student firstname is required")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "The student lastname is required")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "The email address is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [Display(Name = "Email Address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "The phone number is required")]
        [Display(Name = "Phone No")]
        public string PhoneNo { get; set; }

        [Required]
        [Display(Name = "Username")]
        public string Username { get; set; }

        [Required]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*\W)[a-zA-Z\d\W]{8,}$",
        ErrorMessage = "Password must have at least one non-alphanumeric character, one lowercase letter, one uppercase letter, and be at least 8 characters long.")]
        //[StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        [Display(Name = "User Type")]
        public string UserType { get; set; }

        [Required]
        [MaxFileSize(5 * 1024 * 1024)]
        [Display(Name = "Profile image")]
        public IFormFile? ProfileImage { get; set; }
        public bool EmailConfirmed { get; set; }
    }

}
