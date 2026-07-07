using GymManagementSystem.DAL.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymManagementSystem.DAL.Data.Configurations
{
    internal class BookingConfiguration : IEntityTypeConfiguration<Booking>  //member / session
    {
        public void Configure(EntityTypeBuilder<Booking> builder)
        {
            builder.Ignore(x => x.Id);  //for search 

            builder.Property(x => x.CreatedAt)
                   .HasColumnName("BookingDate")
                   .HasDefaultValueSql("GetDate()");

            builder.HasOne(X=>X.Session)
                .WithMany(X => X.SessionsBookings)
                .HasForeignKey(x => x.SessionId);

            builder.HasOne(X=>X.Member)
                .WithMany(X => X.MemberBookings)
                .HasForeignKey(x => x.MemberId);

            builder.HasKey(x => new { x.SessionId, x.MemberId });   //composite key
        }
    }
}
