using CapstoneProject.Models.Account_Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProfanityFilter;
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
        public ReviewInformation Get3Reviews()
        {
            return context.Reviews.FirstOrDefault(n => n.ReviewID < 3);
        }

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
            Debug.WriteLine(ri.Review);
            bool result = test.TestProfanity(ri.Review);
            if (result == false)
            {
                context.Reviews.Add(ri);
                context.SaveChanges();
            }
            else
            {
                Debug.WriteLine("Contained Profanity");
            }
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

namespace ProfanityFilter
{
    public class test
    {
        public static bool TestProfanity(string review)
        {
            var filter = new ProfanityFilter();
            try
            {
                Assert.IsTrue(filter.IsProfanity(review));
            }
            catch (Exception ex) 
            {
                Debug.WriteLine(ex);
                return true;
            }
            return false;
            
        }
    }
}
