using CapstoneProject.Models.Account_Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CapstoneProject.Models
{
    public class CapstoneRepository : IAppointmentRepository, IAccountRepository, IReviewRepository
    {
        private Capstone_DBContext context;
        public CapstoneRepository(Capstone_DBContext ctx)
        {
            context = ctx;
        }
        public IQueryable<AppointmentInfo> GetAllAppointments => context.Appointments.OrderBy(a => a.Time.Hour);
        public IQueryable<AccountInformation> GetAllAccounts => context.Accounts;
        public IQueryable<ReviewInformation> GetAllReviews => context.Reviews;

        public AppointmentInfo GetAppointment(int id)
        {
            return context.Appointments.FirstOrDefault(t => t.AppointmentID == id);
        }

        public void AddAppointment(AppointmentInfo ai)
        {
            context.Appointments.Add(ai);
            context.SaveChanges();
        }

        public void AddUser(AccountInformation ai)
        {
            context.Accounts.Add(ai);
            context.SaveChanges();
        }

        public void AddReview(ReviewInformation ri) 
        { 
            context.Reviews.Add(ri);
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
            var res = context.Appointments.FirstOrDefault(n => n.AppointmentID == id);
            context.Appointments.Remove(res);
            context.SaveChanges();
        }
    }
}
