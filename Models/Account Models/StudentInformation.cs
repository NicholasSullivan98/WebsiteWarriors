using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations;

namespace CapstoneProject.Models.Account_Models
{
    [PrimaryKey(nameof(StudentID))]
    public class StudentInformation
    {
        public int StudentID { get; set; }

        public int ParentAccountID { get; set; }

        [Required(ErrorMessage = "Please, enter your child's first name")]
        public string StudentFirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please, enter your child's last name")]
        public string StudentLastName { get; set; } = string.Empty;
    }
}
