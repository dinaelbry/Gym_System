using GymSystem.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.DAL.Data.Configrations
{
    internal class SessionConfigration : IEntityTypeConfiguration<Session>
    {
        public void Configure(EntityTypeBuilder<Session> builder)
        {
            builder.ToTable(tb =>
            {
                tb.HasCheckConstraint("SessionCapacityCheck", "Capacity between 1 and 25");
                tb.HasCheckConstraint("SessionEndDateCheck", "EndTime > StartTime");
            });

            builder.HasOne(x => x.Trainer)
                .WithMany(x => x.Sessions)
                .HasForeignKey(x => x.TrainerId);
            builder.HasOne(x => x.Category)
                .WithMany(x => x.Sessions)
                .HasForeignKey(x => x.CategoryId);
        }
    }
}
