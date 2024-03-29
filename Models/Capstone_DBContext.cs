﻿using CapstoneProject.Models.Account_Models;
using Microsoft.EntityFrameworkCore;

namespace CapstoneProject.Models
{
    public class Capstone_DBContext : DbContext
    {
        public Capstone_DBContext(DbContextOptions<Capstone_DBContext> options) : base(options) { }

        public DbSet<AppointmentInfo> Appointments { get; set; }
        public DbSet<AccountInformation> Accounts { get; set; }
        public DbSet<ReviewInformation> Reviews { get; set; }
        public DbSet<EmailConfirmationInformation> Confirmations { get; set; }
        public DbSet<StudentInformation> Students { get; set; }
    }
}
