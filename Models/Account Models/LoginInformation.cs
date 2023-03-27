using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace CapstoneProject.Models.Account_Models
{
    public class LoginInformation
    {
        [Required(ErrorMessage = "Please, enter your Email")]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please, enter your Password")]
        public string Password { get; set; } = string.Empty;
    }
}
