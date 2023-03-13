namespace SchoolLIbrary.Models
{
    public class MaterialModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Publisher { get; set; }
        public int Year { get; set; }
        public string Genre  { get; set; } // Such as video, Journal, Textbook (all E-materials must be in PDF format
        public string MaterialType { get; set; } //Hard copy or Electronic
        public string Falculty { get; set; }
        public string Department { get; set; }
        public int? Quantity { get; set; }
        public DateTime DateCreated { get; set; }
        public string? MaterialUrl { get; set; }
        public ApplicationUser? User { get; set; }
    }
}
