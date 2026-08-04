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
    public class MemberConfigration : GymUserConfigration<Member>, IEntityTypeConfiguration<Member>
    {
        public new void Configure(EntityTypeBuilder<Member> builder)
        {
            builder.Property(x => x.CreatedAt).HasColumnName("JoinDate").HasDefaultValueSql("GETDATE()");
            builder.HasOne(x => x.HealthRecord)
                .WithOne(x => x.Member)
                .HasForeignKey<HealthRecord>(x => x.MemberId);
            base.Configure(builder);
        }
    }
}
