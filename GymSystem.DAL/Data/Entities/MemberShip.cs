using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.DAL.Data.Entities
{
    public class MemberShip:BaseEntity
    {
        public DateTime EndDate { get; set; }


        public Plan Plan { get; set; } = null!;
        public int PlanId { get; set; }
        public Member Member { get; set; } = null!;
        public int MemberId { get; set; }

        [NotMapped]
        public bool IsActive => EndDate > DateTime.Now;
        [NotMapped]
        public string Status => IsActive ? "Active" : "Expired";


    }
}
