using GymSystem.DAL.Data.Configrations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using GymSystem.DAL.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace GymSystem.DAL.Data.Contexts
{
    public class GymDbContext: IdentityDbContext<ApplicationUser>
    {
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer(
        //        @"Server=.\DINA;Database=GymSystem;Trusted_Connection=True;TrustServerCertificate=True");
        //}
        public GymDbContext(DbContextOptions<GymDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            modelBuilder.Entity<ApplicationUser>()
                .HasOne(u => u.Member)
                .WithOne()
                .HasForeignKey<ApplicationUser>(u => u.MemberId)
                .OnDelete(DeleteBehavior.SetNull);

            base.OnModelCreating(modelBuilder);
        }
        public DbSet<Plan> Plans { get; set; }
        public DbSet<Trainer> Trainers { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<HealthRecord> HealthRecords { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<MemberShip> Memberships { get; set; }
        public DbSet<Session> Sessions { get; set; }
    }
}
