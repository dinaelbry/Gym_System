using Microsoft.EntityFrameworkCore;
using MVC_Sessions.Configurations;
using MVC_Sessions.Models;

namespace MVC_Sessions.Contexts
{
    public class GymDbContext : DbContext
    {
        public DbSet<Plan> Plans { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=.;Database=GymSystemDB;Integrated Security=True;TrustServerCertificate=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration<Plan>(new PlanConfiguration());
        }
    }
}

