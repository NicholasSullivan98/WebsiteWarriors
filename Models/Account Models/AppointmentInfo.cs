using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace CapstoneProject.Models
{
    [PrimaryKey(nameof(AppointmentID))]
    public class AppointmentInfo
    {
        public int AppointmentID { get; set; }

        [Required(ErrorMessage = "The Name Field is Required.")]
        public string Name { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "The Phone Number Field is Required.")]
        [Phone]
        [RegularExpression("^(?:\\(?)(\\d{3})(?:[-\\).\\s]?)(\\d{3})(?:[-\\.\\s]?)(\\d{4})(?!\\d)", ErrorMessage = "Phone Number is not in correct format (123-456-7890)")]
        public string PhoneNumber { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "The Course Level Field is Required.")]
        public string CourseLevel { get; set; } = string.Empty;

        [Required(ErrorMessage = "The Date Field is Required.")]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "The Time Field is Required.")]
        public DateTime Time { get; set; }

    }


	public class AppointmentDataSource : IDataSource
	{
		public IEnumerable<AppointmentInfo> Appointment =>
			new AppointmentInfo[]
			{
				 new AppointmentInfo{
				Name = "Andy",
				PhoneNumber = "6479872984",
				CourseLevel = "Beginner",
				Date = new System.DateTime(2023, 3, 21) ,
				Time = new System.DateTime(2023, 3, 21,15,30,0)
				}
			};

	}
}
