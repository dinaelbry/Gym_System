using System.ComponentModel.DataAnnotations.Schema;

namespace GymManagementSystem.DAL.Data.Entities
{
    public class MemberShip : BaseEntity
    {
        #region Relationships
        public Member Member { get; set; } = default!;
        public int MemberId { get; set; }
        public Plan Plan { get; set; } = default!;
        public int PlanId { get; set; } 
        #endregion

        //StartDate = CreatedAt Of BaseEntity
        public DateTime EndDate { get; set; }
        [NotMapped] 
        public string Status => EndDate > DateTime.Now ? "Active" : "Expired";   //computed
        [NotMapped] 
        public bool IsActive => EndDate > DateTime.Now;
    }
}
