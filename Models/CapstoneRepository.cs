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
        public IQueryable<AppointmentInfo> GetAllAppointments => context.Appointments.OrderBy(a => a.Time.Hour);

        public AppointmentInfo GetAppointment(int id)
        {
            return context.Appointments.FirstOrDefault(t => t.AppointmentID == id);
        }

        public void AddAppointment(AppointmentInfo ai)
        {
            Debug.WriteLine("Inside CapstoneRepository");
            context.Appointments.Add(ai);
            context.SaveChanges();
        }

        public AppointmentInfo UpdateAppointment(AppointmentInfo ai, int id)
        {
            var res = context.Appointments.FirstOrDefault(n => n.AppointmentID == id);
            res.Name = ai.Name;
            res.PhoneNumber = ai.PhoneNumber;
            res.CourseLevel = ai.CourseLevel;
            res.Date = ai.Date;
            res.Time = ai.Time;
            context.SaveChanges();
            return res;

        }

        public void DeleteAppointment(int id)
        {

            Debug.WriteLine("id: "+id);
            var res = context.Appointments.FirstOrDefault(n => n.AppointmentID == id);
            Debug.WriteLine("res: " + res);
            Debug.WriteLine("res AppointmentName: " + res.Name);
            context.Appointments.Remove(res);
            context.SaveChanges();
        }
    }
}
