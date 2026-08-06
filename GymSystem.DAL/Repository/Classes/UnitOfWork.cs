using GymSystem.DAL.Data.Contexts;
using GymSystem.DAL.Entities;
using GymSystem.DAL.Repository.Interfaces;


namespace GymSystem.DAL.Repository.Classes
{
    public class UnitOfWork : IUnitOfWork
    {
        public IMembershipRepository MembershipRepository { get; }
        public ISessionRepository SessionRepository { get; }

        public IBookingRepository BookingRepository { get; }

        private readonly Dictionary<string, object> repositories = [];
        private readonly GymDbContext _dbContext;
        public UnitOfWork(GymDbContext dbContext,
            IMembershipRepository membershipRepository,
            ISessionRepository sessionRepository,
            IBookingRepository bookingRepository)
        {
            _dbContext = dbContext;
            MembershipRepository = membershipRepository;
            SessionRepository = sessionRepository;
            BookingRepository = bookingRepository;
        }

        public async Task<int> CompleteAsync(CancellationToken ct = default)
       => await _dbContext.SaveChangesAsync(ct);

        

        public IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity, new()
        {
            var TypeName = typeof(TEntity).Name;
            if (repositories.TryGetValue(TypeName, out object? OldRepository))
            {
                return (IGenericRepository<TEntity>)OldRepository;
            }
            var NewRepository = new GenericRepository<TEntity>(_dbContext);
            repositories[TypeName] = NewRepository;
            return NewRepository;
        }


    }
}
