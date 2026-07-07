namespace GymManagementSystem.DAL.Data.Entities
{
    public class Category : BaseEntity
    {
        public string CategoryName { get; set; } = default!;

        #region realationships
        public ICollection<Session> Sessions { get; set; } = new HashSet<Session>();

        #endregion  
    }
    }
