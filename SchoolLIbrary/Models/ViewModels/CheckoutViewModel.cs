namespace SchoolLIbrary.Models.ViewModels
{
    public class CheckoutViewModel
    {
        public int MaterialId { get; set; }
        public string MaterialTitle { get; set; }
        public int QuantityLeft { get; set; }
        public string BorrowerName { get; set; }
        public DateTime CheckoutDate { get; set; }
        public DateTime DueDate { get; set; }
        public string CheckoutCode { get; set; }
    }

}
