using GymSystem.DAL.Data.Contexts;
using GymSystem.DAL.Entities;
using GymSystem.DAL.Repository.Interfaces;


namespace GymSystem.DAL.Repository.Classes
{
    public class UnitOfWork : IUnitOfWork
    {
        public IMembershipRepository MembershipRepository { get; }


      private readonly Dictionary<string, object> _Repos = new();
      private readonly GymDbContext dbContext;
        public UnitOfWork(GymDbContext dbContext, IMembershipRepository membershipRepository)
        {
            this.dbContext = dbContext;
            MembershipRepository = membershipRepository;
        }

        public async Task<int> CompleteAsync(CancellationToken ct = default)
       => await dbContext.SaveChangesAsync(ct);

        

        public IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity, new()
        {
            var TypeName = typeof(TEntity).Name;
            if (_Repos.TryGetValue(TypeName, out object? OldRepository))
            {
                return (IGenericRepository<TEntity>)OldRepository;
            }
            var NewRepository = new GenericRepository<TEntity>(dbContext);
            _Repos[TypeName] = NewRepository;
            return NewRepository;
        }


    }
}
