using System.ComponentModel.DataAnnotations;

namespace SchoolLIbrary.Models.ViewModels
{
    public class LecturerViewModel
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "The lecturer registration number is required")]
        [Display(Name = "Student RegNo")]
        public string? RegNo { get; set; }

        [Required(ErrorMessage = "The lecturer firstname is required")]
        [Display(Name = "First Name")]
        public string? FirstName { get; set; }

        [Required(ErrorMessage = "The lecturer lastname is required")]
        [Display(Name = "Last Name")]
        public string? LastName { get; set; }

        [Required(ErrorMessage = "The email address is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [Display(Name = "Email Address")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "The phone number is required")]
        [Display(Name = "Phone No")]
        public string? PhoneNo { get; set; }

        [Required]
        [Display(Name = "Username")]
        public string? Username { get; set; }

        [Display(Name = "Profile Image")]
        public string? ProfileImageUrl { get; set; }

        [Required]
        [Display(Name = "User Type")]
        public string? UserType { get; set; }

        [Display(Name = "Faculty")]
        public string? Faculty { get; set; }

        [Display(Name = "Department")]
        public string? Department { get; set; }

        [Display(Name = "Profile image")]
        public IFormFile? ProfileImage { get; set; }
    }
}
