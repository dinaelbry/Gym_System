namespace GymManagementSystem.DAL.Data.Entities
{
    public class Member : GymUser
    {
        public string? Photo { get; set; }= default!;

        #region Relationships
        public HealthRecord HealthRecord { get; set; } = default!;

        public ICollection<MemberShip> MembershipPlans = new HashSet<MemberShip>();

        public ICollection<Booking> MemberBookings = new HashSet<Booking>();
        #endregion
    }
}
