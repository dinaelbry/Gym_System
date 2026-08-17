using GymSystem.DAL.Data.Contexts;
using GymSystem.DAL.Entities;
using GymSystem.DAL.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;


namespace GymSystem.DAL.Repository.Classes
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity, new()
    {
        private readonly GymDbContext _dbcontext;
        private readonly DbSet<TEntity> _set;

        public GenericRepository(GymDbContext dbcontext)
        {
            _dbcontext = dbcontext;
            _set = _dbcontext.Set<TEntity>();
        }

        public void Add(TEntity entity, CancellationToken ct = default) => _set.Add(entity);
        public void Update(TEntity entity, CancellationToken ct = default) => _set.Update(entity);
        public void Delete(TEntity entity, CancellationToken ct = default) => _set.Remove(entity);

        public Task<bool> Any(Expression<Func<TEntity, bool>> predicate, CancellationToken ct = default)
             => _set.AsNoTracking().AnyAsync(predicate, ct);

        public async Task<int> CompleteAsync(CancellationToken ct = default)
            => await _dbcontext.SaveChangesAsync(ct);




        public Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, bool isTracked = false, CancellationToken ct = default)
        {
            IQueryable<TEntity> query = isTracked ? _set : _set.AsNoTracking();
            return query.FirstOrDefaultAsync(predicate, ct);
        }

        public async Task<IEnumerable<TEntity>> GetAll(bool isTracked, CancellationToken ct = default)
        {
            IQueryable<TEntity> query = isTracked ? _set : _set.AsNoTracking();
            return await query.ToListAsync(ct);
        }

        public async Task<TEntity?> GetById(int id, CancellationToken ct = default)
             => await _set.FirstOrDefaultAsync(p => p.Id == id, ct);

        public Task<int> CountAsync(Expression<Func<TEntity, bool>>? predicate = null, CancellationToken ct = default)
            => predicate is null ? _set.AsNoTracking().CountAsync(ct) : _set.AsNoTracking().CountAsync(predicate, ct);

       
    }
}
