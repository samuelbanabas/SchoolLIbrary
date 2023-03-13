using System.ComponentModel.DataAnnotations;

namespace SchoolLIbrary.Models.ViewModels
{
    public class RegistrationCodeViewModel
    {
        public int Id { get; set; }
         
        public string? StaffNo { get; set; }
       
        public string? Code { get; set; }

        public DateTime DateGenerated { get; set; }
    }
}
