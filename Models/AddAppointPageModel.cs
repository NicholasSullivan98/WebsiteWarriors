using CapstoneProject.Models.Account_Models;

namespace CapstoneProject.Models
{
    public class AddAppointPageModel
    {
        public IEnumerable<AppointmentInfo> Appointments { get; set; }
        public AppointmentInfo Appointment { get; set; }
        public AccountInformation Accounts { get; set; }
        public IEnumerable<StudentInformation> Students { get; set; }
    }
}
