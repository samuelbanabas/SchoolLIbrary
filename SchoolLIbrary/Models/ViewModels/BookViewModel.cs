﻿using System.ComponentModel.DataAnnotations;

namespace SchoolLIbrary.Models.ViewModels
{
    public class BookViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Author { get; set; }

        [Required]
        public string Publisher { get; set; }

        [Required]
        public int Year { get; set; }

        [Required]
        public string Genre { get; set; }

        [Required]
        public string MaterialType { get; set; }

        [Required]
        public string Falculty { get; set; }

        [Required]
        public string Department { get; set; }

        [Required]
        public int? Quantity { get; set; }

        [Required]
        public string DateCreated { get; set; }
    }
}
