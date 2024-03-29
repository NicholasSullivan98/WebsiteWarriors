﻿using CapstoneProject.Models.Account_Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Identity;
using ProfanityFilter;
using System.Diagnostics;
using System.Security.Cryptography;

namespace CapstoneProject.Models
{
    public class CapstoneRepository : IAppointmentRepository, IAccountRepository, IReviewRepository, IConfirmationRepository, IStudentRepository
    {
        private Capstone_DBContext context;
        public CapstoneRepository(Capstone_DBContext ctx)
        {
            context = ctx;
        }
        public IQueryable<AppointmentInfo> GetAllAppointments => context.Appointments.OrderBy(a => a.Time.Hour);
        public IQueryable<AccountInformation> GetAllAccounts => context.Accounts;
        public IQueryable<ReviewInformation> GetAllReviews => context.Reviews;
        public IQueryable<EmailConfirmationInformation> GetAllConfirmations => context.Confirmations;
        public IQueryable<StudentInformation> GetAllStudents => context.Students;

        public AccountInformation GetLoggedInAccountInfo(int id)
        {
            return context.Accounts.FirstOrDefault(n => n.AccountID == id);
        }

        public ReviewInformation Get3Reviews()
        {
            return context.Reviews.FirstOrDefault(n => n.ReviewID < 3);
        }

        public AppointmentInfo GetAppointment(int id)
        {
            return context.Appointments.FirstOrDefault(t => t.AppointmentID == id);
        }

        public IQueryable<AppointmentInfo> GetUnpaidAppointments()
        {
            return context.Appointments.Where(t => t.Paid == false);
        }

        public StudentInformation GetStudent(int id)
        {
            return context.Students.FirstOrDefault(t => t.StudentID == id);
        }

        public void AddAppointment(AppointmentInfo ai)
        {
            context.Appointments.Add(ai);
            context.SaveChanges();
        }

        private readonly RandomNumberGenerator _rng;

        public void AddUser(AccountInformation ai)
        {
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(ai.Password);

            //Debug.WriteLine(ai.Password);
            //Debug.WriteLine(passwordHash);

            ai.Password = passwordHash;
            ai.PasswordConformation = passwordHash;

            context.Accounts.Add(ai);
            context.SaveChanges();
        }

        public void AddConfirmation(EmailConfirmationInformation ci)
        {
            context.Confirmations.Add(ci);
            context.SaveChanges();
        }

        public void AddStudent(StudentInformation si)
        {
            context.Students.Add(si);
            context.SaveChanges();
        }

        public void DeleteConfirmation(int id)
        {
            var res = context.Confirmations.FirstOrDefault(n => n.ConfirmationID == id);
            context.Confirmations.Remove(res);
            context.SaveChanges();
        }

        public AccountInformation UpdateUser(AccountInformation ai, int id)
        {
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(ai.Password);

            var res = context.Accounts.FirstOrDefault(n => n.AccountID == id);
            res.Password = passwordHash;
            res.PasswordConformation = passwordHash;
            context.SaveChanges();
            return res;
        }

        public void AddReview(ReviewInformation ri) 
        {
            Debug.WriteLine(ri.Review);
            bool result = test.TestProfanity(ri.Review);
            bool titleResult = test.TestProfanity(ri.ReviewTitle);
            if (result == false && titleResult == false)
            {
                context.Reviews.Add(ri);
                context.SaveChanges();
            }
            else
            {
                Debug.WriteLine("Contained Profanity");
            }
        }

        public void DeleteReview(int id)
        {
            var res = context.Reviews.FirstOrDefault(n => n.ReviewID == id);
            context.Reviews.Remove(res);
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
            res.Email = ai.Email;
            res.ParentName = ai.ParentName;
            res.Paid = ai.Paid;
            context.SaveChanges();
            return res;

        }

        public AppointmentInfo UpdateAppointmentPayment(int id)
        {
            var res = context.Appointments.FirstOrDefault(n => n.AppointmentID == id);
            
            if (res.Paid == false)
            {
                res.Paid = true;
            }
            else
            {
                res.Paid = false;
            }
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

            var swearList = filter.DetectAllProfanities(review);

            if (swearList.Count == 0)
            {
                return false;
            }
            else
            {
                return true;
            }        
        }
    }
}


