namespace SchoolLIbrary.Models
{
    public class RegistrationCodeModel
    {
        public int Id { get; set; }
        public string? StaffNo { get; set; }
        public string? Code { get; set; }
        public DateTime DateGenerated { get; set; }
    }
}
