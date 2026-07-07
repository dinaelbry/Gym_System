namespace GymManagementSystem.DAL.Data.Entities
{
    public class Booking : BaseEntity
    {
        #region relationships
        public Member Member { get; set; } = default!;
        public int MemberId { get; set; }

        public Session Session { get; set; } = default!;
        public int SessionId { get; set; }

        #endregion
        // BookingDate  = CreatedAt Of BaseEntity
        public bool IsAttended { get; set; }
    }
}
  