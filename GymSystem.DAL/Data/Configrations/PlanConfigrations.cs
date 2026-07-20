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
    public class PlanConfigrations : IEntityTypeConfiguration<Plan>
    {
        public void Configure(EntityTypeBuilder<Plan> builder)
        {
            builder.Property(p => p.Name).HasColumnType("varchar(100)");
            builder.Property(p => p.Description).HasMaxLength(200);
            builder.Property(p => p.Price).HasPrecision(10, 2); // decimal (10,2)
            builder.Property(p => p.CreatedAt).HasDefaultValueSql("GETDATE()");
            builder.ToTable(tb => {
                tb.HasCheckConstraint("DurationCheckValue", "Duration Between 1 and 365");
            });

        }
    }
}
