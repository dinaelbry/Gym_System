using GymSystem.DAL.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.DAL.Data.Configrations
{
    internal class MemberShipConfigration:IEntityTypeConfiguration<MemberShip>
    {
        public void Configure(EntityTypeBuilder<MemberShip> builder)
        {
            builder.HasKey(m => m.Id);
            builder.Property(x=>x.CreatedAt)
                .HasColumnName("StartDate")
                .HasDefaultValueSql("GETDATE()");

            builder.HasOne(m=>m.Plan)
                .WithMany(p => p.MemberShips)
                .HasForeignKey(m => m.PlanId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(m => m.Member)
                .WithMany(m => m.MemberShips)
                .HasForeignKey(m => m.MemberId)
                .OnDelete(DeleteBehavior.Restrict);
        }

       
    }
}
