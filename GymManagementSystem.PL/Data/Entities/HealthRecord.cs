using System.ComponentModel.DataAnnotations;

namespace GymManagementSystem.DAL.Data.Entities
{
    public class HealthRecord : BaseEntity
    {
        public decimal Height { get; set; }
        public decimal Weight { get; set; }
        [Required,MaxLength(5)]
        public string BloodType { get; set; } = default!;
        public string? Note { get; set; }

        #region Relationships

        public Member Member { get; set; } = default!;
        public int MemberId { get; set; }
        #endregion
    }
}
