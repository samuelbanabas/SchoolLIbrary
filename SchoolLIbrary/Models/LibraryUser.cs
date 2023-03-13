using Microsoft.AspNetCore.Identity;

namespace SchoolLIbrary.Models
{
    public class LibraryUser 
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string RegNo { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNo { get; set; }
        public string? Faculty { get; set; }
        public string? Department { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string UserType { get; set; }

        public virtual ICollection<CheckoutModel>? BorrowedBooks { get; set; }
    }    
}
