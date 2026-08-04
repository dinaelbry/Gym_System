using GymSystem.BLL.Common;
using GymSystem.BLL.Services.Interfaces;
using GymSystem.BLL.ViewModels.MembershipViewModels;
using GymSystem.DAL.Entities;
using GymSystem.DAL.Repository.Interfaces;



namespace GymSystem.BLL.Services.Classes
{
    public class MembershipService : IMembershipService
    {
        private readonly IUnitOfWork unitOfWork;

    public MembershipService(IUnitOfWork unitOfWork)
    {
        this.unitOfWork = unitOfWork;
    }
    public async Task<Result> CreateMembershipAsync(CreateMembershipViewModel model, CancellationToken ct = default)
        {
            var memberExists = await unitOfWork.GetRepository<Member>().Any(m => m.Id == model.MemberId, ct);
            if (!memberExists) return Result.NotFound("Member not found.");

            var plan = await unitOfWork.GetRepository<Plan>().GetById(model.PlanId, ct);
            if (plan is null) return Result.NotFound("Plan not found.");
            if (!plan.IsActive) return Result.Fail("Cannot subscribe to an inactive plan.");

            var StartDate = model.StartDate ?? DateTime.Now;

            var membership = new MemberShip
            {
                MemberId = model.MemberId,
                PlanId = model.PlanId,
                CreatedAt = StartDate,
                EndDate = StartDate.AddDays(plan.Duration),
            };

            var hasActive = await unitOfWork.MembershipRepository
                .Any(m => m.MemberId == model.MemberId && m.EndDate > DateTime.Now, ct);
            if (hasActive) return Result.Fail("Member already has an active membership.");


            unitOfWork.GetRepository<MemberShip>().Add(membership, ct);
            var result = await unitOfWork.CompleteAsync(ct);
            return result > 0 ? Result.Ok() : Result.Fail("Failed To Create New Membership");
        }

        public async Task<Result> DeleteActiveMembershipAsync(int id, CancellationToken ct = default)
        {
            var membership = await unitOfWork.GetRepository<MemberShip>().GetById(id, ct);
            if (membership is null) return Result.NotFound("Membership not found.");

            unitOfWork.GetRepository<MemberShip>().Delete(membership, ct);
            var result = await unitOfWork.CompleteAsync(ct);

            return result > 0 ? Result.Ok() : Result.Fail("Failed to cancel membership.");
        }


        public async Task<IEnumerable<MembershipViewModel>> GetAllMembershipsAsync(CancellationToken ct = default)
        {
            var memberships = await unitOfWork.MembershipRepository.GetAllMembershipsWithMemberAndPlanAsync(ct: ct);

            return memberships.Select(m => new MembershipViewModel
            {
                Id = m.Id,
                MemberName = m.Member.Name!,
                PlanName = m.Plan.Name!,
                StartDate = m.CreatedAt,
                EndDate = m.EndDate,
                Status = m.EndDate > DateTime.Now ? "Active" : "Expired"
            });

        }


        public async Task<IEnumerable<Plan>> GetPlansForDropDownAsync(CancellationToken ct = default)
        {
            var plans = await unitOfWork.GetRepository<Plan>().GetAll(false, ct);
            return plans.Where(p => p.IsActive);
        }

        public async Task<IEnumerable<Member>> GetMembersForDropDownAsync(CancellationToken ct = default)
            => await unitOfWork.GetRepository<Member>().GetAll(false, ct);
    
}
}