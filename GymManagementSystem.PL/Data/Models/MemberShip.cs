namespace GymManagementSystem.DAL.Data.Models
{
    public class MemberShip : BaseEntity
    {
        public Member? Member { get; set; }
        public int MemberId { get; set; }
        public Plan? Plan { get; set; }
        public int PlanId { get; set; }

        //StartDate = CreatedAt Of BaseEntity
        public DateTime EndDate { get; set; }

        public string Status => EndDate > DateTime.Now ? "Active" : "Expired";
        public bool IsActive => EndDate > DateTime.Now;
    }
}
