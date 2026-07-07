using GymManagementSystem.DAL.Data.Entities.Enums;

namespace GymManagementSystem.DAL.Data.Entities
{
    public class Trainer : GymUser
    {
        public Specialties Specialties { get; set; }
        public DateTime HiringDate { get; set; }


        #region relationships
        public ICollection<Session> Sessions { get; set; } = new HashSet<Session>();
        public int TrainerId { get; set; } = default!;
        #endregion   
    }
    }






