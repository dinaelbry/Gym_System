using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MVC_Sessions.Models;

namespace MVC_Sessions.Configurations
{
    public class PlanConfiguration : IEntityTypeConfiguration<Plan>
    {
        public void Configure(EntityTypeBuilder<Plan> builder)
        {
            builder.Property(p => p.Name).HasColumnType("varchar").HasMaxLength(50);
            builder.Property(p => p.Description).HasMaxLength(150);
            builder.Property(p => p.Price).HasPrecision(10, 2);
            builder.Property(p => p.CreatedAt).HasDefaultValueSql("GETDATE()");
            builder.ToTable(p =>
            {
                p.HasCheckConstraint("DurationCheck", "Duration Between 1 and 365");
            });
        }
    }
}
