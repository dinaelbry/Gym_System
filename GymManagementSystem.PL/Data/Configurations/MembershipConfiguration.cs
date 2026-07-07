using GymManagementSystem.DAL.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymManagementSystem.DAL.Data.Configurations
{
    internal class MembershipConfiguration : IEntityTypeConfiguration<MemberShip>  // relation member and plan
    {
        public void Configure(EntityTypeBuilder<MemberShip> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.CreatedAt)
                   .HasColumnName("StartDate")
                   .HasDefaultValueSql("GETDATE()");
            
            builder.HasOne(m=>m.Plan)
                .WithMany(p=>p.MembershipPlans)
                .HasForeignKey(m=>m.PlanId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(m => m.Member)
                .WithMany(me => me.MembershipPlans)
                .HasForeignKey(m => m.MemberId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
