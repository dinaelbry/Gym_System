using GymSystem.DAL.Entities;
using GymSystem.DAL.Repository.Interfaces;
using GymSystem.DAL.Data.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace GymSystem.DAL.Repository.Classes
{
    public class MembershipRepository : GenericRepository<MemberShip>, IMembershipRepository
    {
        private readonly GymDbContext _dbContext;

        public MembershipRepository(GymDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<MemberShip>> GetAllMembershipsWithMemberAndPlanAsync(Expression<Func<MemberShip, bool>>? predicate = null,
           CancellationToken ct = default)
        {
            IQueryable<MemberShip> query = _dbContext.Memberships
                .AsNoTracking()
                .Include(m => m.Plan)
                .Include(m => m.Member);

            if (predicate is not null) query = query.Where(predicate);

            return await query.ToListAsync(ct);
        }

    }
}