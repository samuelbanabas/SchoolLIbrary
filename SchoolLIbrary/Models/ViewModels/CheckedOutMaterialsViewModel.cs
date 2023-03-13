namespace SchoolLIbrary.Models.ViewModels
{
    public class CheckedOutMaterialsViewModel
    {
        public int Id { get; set; }
        public int MaterialId { get; set; }
        public string MaterialTitle { get; set; }
        public string MaterialType { get; set; }
        public int Quantity { get; set; }
        public int QuantityLeft { get; set; }
        public string UserName { get; set; }
        public string RegNo { get; set; }
        public string PhoneNumber { get; set; }
        public string Status { get; set; }
        public string CheckoutCode { get; set; }
        public decimal Fine { get; set; }
        public DateTime CheckoutDate { get; set; }
        public DateTime ReturnDate { get; set; }
    }
}
