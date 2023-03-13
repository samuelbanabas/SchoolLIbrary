namespace SchoolLIbrary.Models.ViewModels
{
    public class MaterialsViewModel
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? MaterialType { get; set; }
        public int? Quantity { get; set; }
        public int? QuantityLeft { get; set; }
        public string? MaterialUrl { get; set; }
    }
}
