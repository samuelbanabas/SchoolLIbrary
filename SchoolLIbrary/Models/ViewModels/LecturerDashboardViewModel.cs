namespace SchoolLIbrary.Models.ViewModels
{
    public class LecturerDashboardViewModel
    {
        public ApplicationUser User { get; set; }
        public List<MaterialModel> Material { get; set; }
        public string? ProfileImageUrl { get; set; }
    }
}
