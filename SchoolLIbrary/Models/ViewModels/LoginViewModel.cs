﻿using System.ComponentModel.DataAnnotations;

namespace SchoolLIbrary.Models.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Username")]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
        public string? Role { get;  set; }

        //public string UserType { get; set; }
    }

}
