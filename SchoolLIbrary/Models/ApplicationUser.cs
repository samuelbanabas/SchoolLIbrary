using Microsoft.AspNetCore.Identity;

namespace SchoolLIbrary.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? ProfileImageUrl { get; set; }
        public string? RegNo { get; set; }
        public string? Faculty { get; set; }
        public string? Department { get; set; }
        public string? Password { get; set; }
        public string? UserType { get; set; }
        //public bool ConfirmedEmail { get; set; }
    }
}
