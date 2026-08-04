using GymSystem.DAL.Entities;
using System.Linq.Expressions;


namespace GymSystem.DAL.Repository.Interfaces
{
    public interface IGenericRepository<TEntity> where TEntity : BaseEntity,new()
    {
        Task<IEnumerable<TEntity>> GetAll(bool isTracked, CancellationToken ct = default);
        Task<TEntity?> GetById(int id, CancellationToken ct = default);
        void Add(TEntity entity, CancellationToken ct = default);
        void Update(TEntity entity, CancellationToken ct = default);
        void Delete(TEntity entity, CancellationToken ct = default);
        Task<int> CompleteAsync(CancellationToken ct = default);
        Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate,bool isTracked=false ,CancellationToken ct = default);
        Task<bool> Any(Expression<Func<TEntity, bool>> predicate, CancellationToken ct = default);
        Task<int> CountAsync(Expression<Func<TEntity,bool>>? predicate =null ,CancellationToken ct = default);
    }
}
