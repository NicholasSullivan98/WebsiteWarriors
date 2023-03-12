using Microsoft.AspNetCore.Mvc;

namespace CapstoneProject.Models
{
    public interface IAppointmentRepository
    {
        IQueryable<AppointmentInfo> GetAllAppointments { get; }
        public void AddAppointment(AppointmentInfo appointment);
    }
}
