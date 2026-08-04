using GymSystem.DAL.Entities;


namespace GymSystem.DAL.Repository.Interfaces
{
    public interface IUnitOfWork
    {
        //UnitOfWork.GetRepos<Member>().GetAll();

         IMembershipRepository MembershipRepository { get; }

         IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity , new();
         Task<int> CompleteAsync(CancellationToken ct = default);
    }
}
