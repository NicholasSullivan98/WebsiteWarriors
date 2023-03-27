using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace CapstoneProject.Models.Account_Models
{
    [PrimaryKey(nameof(AccountID))]
    public class AccountInformation 
    {
        public int AccountID { get; set; }

        [Required(ErrorMessage = "Please, enter your first name")]
        public string ParentFirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please, enter your last name")]
        public string ParentLastName { get; set; } = string.Empty;
        [Required(ErrorMessage = "Please, enter your child's first name")]
        public string StudentFirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please, enter your child's last name")]
        public string StudentLastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please, enter your email")]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please, enter your phone number")]
        [Phone]
        [RegularExpression("^(?:\\(?)(\\d{3})(?:[-\\).\\s]?)(\\d{3})(?:[-\\.\\s]?)(\\d{4})(?!\\d)", ErrorMessage = "Phone Number is not in correct format (123-456-7890)")]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please, enter your password")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please, confirm your password")]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        public string PasswordConformation { get; set; } = string.Empty;
    }
}
