using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CapstoneProject.Models
{
    public class CapstoneRepository : IAppointmentRepository
    {
        private Capstone_DBContext context;
        public CapstoneRepository(Capstone_DBContext ctx)
        {
            context = ctx;
        }
        public IQueryable<AppointmentInfo> GetAllAppointments => context.Appointments;

        public void AddAppointment(AppointmentInfo ai)
        {
            Debug.WriteLine("Inside CapstoneRepository");
            context.Appointments.Add(ai);
            context.SaveChanges();
        }
    }
}
