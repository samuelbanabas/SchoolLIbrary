using Microsoft.AspNetCore.Identity;

namespace SchoolLIbrary.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? ProfileImageUrl { get; set; }
        //public bool ConfirmedEmail { get; set; }
    }
}
