using System.ComponentModel.DataAnnotations;

namespace SchoolLIbrary.Models.ViewModels
{
    public class MaxFileSizeAttribute : ValidationAttribute
    {
        private readonly int _maxFileSize;

        public MaxFileSizeAttribute(int maxFileSize)
        {
            _maxFileSize = maxFileSize;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var file = value as IFormFile;
            if (file == null)
            {
                return new ValidationResult("The file is required.");
            }

            if (file.Length > _maxFileSize)
            {
                return new ValidationResult($"The maximum allowed file size is {_maxFileSize / 1024 / 1024} MB.");
            }

            return ValidationResult.Success;
        }
    }
}
