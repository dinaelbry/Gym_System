using GymSystem.BLL.Common;
using GymSystem.BLL.ViewModels.MembershipViewModels;
using GymSystem.DAL.Entities;

namespace GymSystem.BLL.Services.Interfaces
{
    public interface IMembershipService
    {

        Task<IEnumerable<MembershipViewModel>> GetAllMembershipsAsync(CancellationToken ct = default);
        Task<Result> CreateMembershipAsync(CreateMembershipViewModel model, CancellationToken ct = default);
        Task<Result> DeleteActiveMembershipAsync(int id, CancellationToken ct = default);
        Task<IEnumerable<PlanSelectListViewModel>> GetPlansForDropDownAsync(CancellationToken ct = default);
        Task<IEnumerable<MemberSelectListViewModel>> GetMembersForDropDownAsync(CancellationToken ct = default);
    }
}
