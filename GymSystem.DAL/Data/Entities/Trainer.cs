using GymSystem.DAL.Data.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.DAL.Data.Entities
{
    public class Trainer: GymUser
    {
        public Specialties Specialize { get; set; }
        public DateTime HireingDate { get; set; }
        public ICollection<Session> Sessions { get; set; } = new HashSet<Session>();
    }
}
