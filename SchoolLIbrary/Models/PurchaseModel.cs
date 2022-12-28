using Microsoft.EntityFrameworkCore;

namespace SchoolLIbrary.Models
{
    public class PurchaseModel
    {
        public int Id { get; set; }
        public MaterialModel Material { get; set; }
        public string Vendor { get; set; }
        [Precision(10, 3)]
        public decimal Cost { get; set; }
        public DateTime PurchaseDate { get; set; }
    }
}
