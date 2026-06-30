namespace GymManagementSystem.DAL.Data.Models
{
    public class Member : GymUser
    {
        public string? Photo { get; set; }

        #region Relationships
        public HealthRecord HealthRecord { get; set; } = default!;

        public ICollection<MemberShip> MemberShips { get; set; } = default!;

        public ICollection<Booking> MemberSessions { get; set; } = default!;
        #endregion
    }
}
