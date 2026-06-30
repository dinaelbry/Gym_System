using GymManagementSystem.DAL.Data.Models.Enums;

namespace GymManagementSystem.DAL.Data.Models
{
    public class Trainer : GymUser
    {
        public Specialties Specialties { get; set; }

        public ICollection<Session> Sessions { get; set; } = default!;
    }
}
