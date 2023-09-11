using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations;

namespace CapstoneProject.Models.Account_Models
{
    [PrimaryKey(nameof(ReviewID))]
    public class ReviewInformation
    {
        public int ReviewID { get; set; }

        public string ReviewerName { get; set;}

        [Required(ErrorMessage = "Please, enter your review")]
        public string Review { get; set;}
        [Required(ErrorMessage = "Please, enter a rating")]
        public int Rating { get; set;}
    }
}
