using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.DAL.Data.Entities
{
    public class Plan : BaseEntity
    {

        public string? Name { get; set; } = null!;
        public decimal Price { get; set; }
        public string? Description { get; set; } = null!;


        public int Duration { get; set; } = 0!;
        public bool IsActive { get; set; } = true;
        public int? CategoryId { get; set; }
public Category? Category { get; set; }
        public ICollection<MemberShip> MemberShips { get; set; } = new HashSet<MemberShip>();
    }
}
