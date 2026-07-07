using GymManagementSystem.DAL.Data.Contexts;
using Microsoft.EntityFrameworkCore;

namespace GymManagementSystem.DAL.Data.Entities
{
    public class Plan : BaseEntity
    {
       
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
        public decimal Price { get; set; }
        public int Duration { get; set; } 
        public bool IsActive { get; set; }

        public ICollection<MemberShip> MembershipPlans = new HashSet<MemberShip>();
    }
}
