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
    public class GymUserConfigration<T> : IEntityTypeConfiguration<T> where T : GymUser // Member / trainer
    {
        public void Configure(EntityTypeBuilder<T> builder)
        {
            builder.Property(x => x.Name).HasColumnType("varchar").HasMaxLength(50);

            builder.Property(x => x.Email).HasColumnType("varchar").HasMaxLength(50);
            builder.Property(x => x.PhoneNumber).HasColumnType("varchar").HasMaxLength(50);
            
            builder.HasIndex(x => x.Email).IsUnique();
            builder.HasIndex(x => x.PhoneNumber).IsUnique();
            builder.ToTable(tb =>
            {
                tb.HasCheckConstraint("EmailCheck", "Email LIKE '_%@_%._%'");
                tb.HasCheckConstraint("PhoneNumberCheck",
                "(PhoneNumber LIKE '010%' OR PhoneNumber LIKE '011%' OR PhoneNumber LIKE '012%' OR PhoneNumber LIKE '015%') AND LEN(PhoneNumber) = 11");
            });
            builder.OwnsOne(x => x.Address, address =>
            {
                address.Property(a => a.City).HasColumnName("City").HasColumnType("varchar").HasMaxLength(50);
                address.Property(a => a.Street).HasColumnName("Street").HasColumnType("varchar").HasMaxLength(50);
                address.Property(a => a.BuildingNumber).HasColumnName("BuildingNumber");
            });
        }
    }

}
