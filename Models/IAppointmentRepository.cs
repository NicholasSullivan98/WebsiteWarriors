using Microsoft.AspNetCore.Mvc;

namespace CapstoneProject.Models
{
    public interface IAppointmentRepository
    {
        IQueryable<AppointmentInfo> GetAllAppointments { get; }
        AppointmentInfo GetAppointment(int id);
        IQueryable<AppointmentInfo> GetUnpaidAppointments();
        public void AddAppointment(AppointmentInfo appointment);
        AppointmentInfo UpdateAppointment(AppointmentInfo ai, int id);
        AppointmentInfo UpdateAppointmentPayment(int id);
        public void DeleteAppointment(int id);
    }
}
