using Microsoft.EntityFrameworkCore;

namespace SchoolLIbrary.Models
{
    public class CheckoutModel
    {
        public int Id { get; set; }
        public MaterialModel Material { get; set; }
        public LibraryUser User { get; set; }
        public string? Status { get; set; }
        [Precision(10,3)]
        public decimal? Fine { get; set; }
        public DateTime CheckoutDate { get; set; }
        public DateTime ReturnDate { get; set; }
    }
}
