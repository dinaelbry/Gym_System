using GymSystem.DAL.Data.Contexts;
using GymSystem.DAL.Data.Entities;
using GymSystem.DAL.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.DAL.Repository.Classes
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity, new()
    {
        private readonly GymDbContext _dbcontext;
        public GenericRepository(GymDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public async Task<int> Add(TEntity entity, CancellationToken ct = default)
        {
             _dbcontext.Set<TEntity>().Add(entity);
            return await _dbcontext.SaveChangesAsync(ct);
        }

        public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken ct = default)
        {
            return await _dbcontext.Set<TEntity>().AnyAsync(predicate, ct);
        }

 

        public async Task<int> Delete(TEntity entity, CancellationToken ct = default)
        {
            var item = await _dbcontext.Set<TEntity>()
                .FirstOrDefaultAsync(x => x.Id == entity.Id, ct);

            if (item == null)
                return 0;

            _dbcontext.Set<TEntity>().Remove(item);
            return await _dbcontext.SaveChangesAsync(ct);
        }

        public Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, bool isTracked = false, CancellationToken ct = default)
        {
            var items = isTracked ? _dbcontext.Set<TEntity>() : _dbcontext.Set<TEntity>().AsNoTracking();
            return items.FirstOrDefaultAsync(predicate, ct);
        }

        public async Task<IEnumerable<TEntity>> GetAll(bool isTracked, CancellationToken ct = default)
        {
            var items = isTracked
                ? _dbcontext.Set<TEntity>()
                : _dbcontext.Set<TEntity>().AsNoTracking();

            return await items.ToListAsync(ct);
        }        

        public async Task<TEntity?> GetById(int id, CancellationToken ct = default)
        {
            var item = await _dbcontext.Set<TEntity>().FirstOrDefaultAsync(p => p.Id == id, ct);
            return item;
        }

        public async Task<int> Update(TEntity entity, CancellationToken ct = default)
        {
            _dbcontext.Set<TEntity>().Update(entity);
            return await _dbcontext.SaveChangesAsync(ct);
        }
    }
}
