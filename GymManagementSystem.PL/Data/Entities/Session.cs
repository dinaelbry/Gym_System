namespace GymManagementSystem.DAL.Data.Entities
{
    public class Session : BaseEntity
    {
        public string Description { get; set; } = default!;
        public int Capacity { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        #region realationships
        public Trainer Trainer { get; set; } = default!;
        //public int TrainerId { get; set; }


        public Category Category { get; set; } = default!;
        public int CategoryId { get; set; }

        public ICollection<Booking> SessionsBookings { get; set; } = new HashSet<Booking>(); 
        #endregion
    }
}