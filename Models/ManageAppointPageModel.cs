using Microsoft.AspNetCore.Mvc;

namespace CapstoneProject.Models
{
    public class ManageAppointPageModel
    {
        public IEnumerable<AppointmentInfo> Appointments { get; set; }
    }
}
