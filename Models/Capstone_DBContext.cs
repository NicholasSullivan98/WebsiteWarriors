using Microsoft.EntityFrameworkCore;

namespace CapstoneProject.Models
{
    public class Capstone_DBContext : DbContext
    {
        public Capstone_DBContext(DbContextOptions<Capstone_DBContext> options) : base(options) { }

        public DbSet<AppointmentInfo> Appointments { get; set; }
    }
}
