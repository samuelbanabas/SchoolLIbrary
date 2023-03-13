namespace SchoolLIbrary.Models.ViewModels
{
    public class StudentDashboardViewModel
    {
        public ApplicationUser User { get; set; }
        public List<CheckoutModel> Checkouts { get; set; }
        public string? ProfileImageUrl { get; set; }
    }
}
