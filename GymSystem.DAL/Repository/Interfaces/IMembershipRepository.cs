using GymSystem.DAL.Entities;
using System.Linq.Expressions;


namespace GymSystem.DAL.Repository.Interfaces
{
    public interface IMembershipRepository : IGenericRepository<MemberShip>
    {
        Task<List<MemberShip>> GetAllMembershipsWithMemberAndPlanAsync(Expression<Func<MemberShip, bool>>? predicate = null, CancellationToken ct = default);
    }
   
}
