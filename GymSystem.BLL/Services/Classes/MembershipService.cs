using AutoMapper;
using GymSystem.BLL.Common;
using GymSystem.BLL.Services.Interfaces;
using GymSystem.BLL.ViewModels.MembershipViewModels;
using GymSystem.DAL.Entities;
using GymSystem.DAL.Repository.Interfaces;
using Microsoft.EntityFrameworkCore.Metadata.Internal;



namespace GymSystem.BLL.Services.Classes
{
    public class MembershipService : IMembershipService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;


        public MembershipService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;

        }
        public async Task<Result> CreateMembershipAsync(CreateMembershipViewModel model, CancellationToken ct = default)
        {
            var memberExists = await unitOfWork.GetRepository<Member>().Any(m => m.Id == model.MemberId, ct);
            if (!memberExists) return Result.NotFound("Member not found.");

            var plan = await unitOfWork.GetRepository<Plan>().GetById(model.PlanId, ct);
            if (plan is null) return Result.NotFound("Plan not found.");
            if (!plan.IsActive) return Result.Fail("Cannot subscribe to an inactive plan.");
          

            var hasActive = await unitOfWork.MembershipRepository
                .Any(m => m.MemberId == model.MemberId && m.EndDate > DateTime.Now, ct);
            if (hasActive) return Result.Fail("Member already has an active membership.");

            var entity = mapper.Map<MemberShip>(model);
            entity.PlanId = plan.Id;
            entity.CreatedAt = model.StartDate ?? DateTime.Now;
            entity.EndDate = entity.CreatedAt.AddDays(plan.Duration);

            unitOfWork.MembershipRepository.Add(entity, ct);
            var result = await unitOfWork.CompleteAsync(ct);
            return result > 0 ? Result.Ok() : Result.Fail("Failed To Create New Membership");
        }

        public async Task<Result> DeleteActiveMembershipAsync(int id, CancellationToken ct = default)
        {
            var active = await unitOfWork.MembershipRepository.FirstOrDefaultAsync(m => m.Id == id && m.EndDate > DateTime.Now, isTracked: true, ct);
            if (active is null) return Result.NotFound("Membership not found.");

            unitOfWork.MembershipRepository.Delete(active, ct);
            var result = await unitOfWork.CompleteAsync(ct);

            return result > 0 ? Result.Ok() : Result.Fail("Failed to cancel membership.");
        }


        public async Task<IEnumerable<MembershipViewModel>> GetAllMembershipsAsync(CancellationToken ct = default)
        {
            var memberships = await unitOfWork.MembershipRepository.GetAllMembershipsWithMemberAndPlanAsync(m => m.EndDate > DateTime.Now, ct);
            return mapper.Map<IEnumerable<MembershipViewModel>>(memberships);
        }


        public async Task<IEnumerable<PlanSelectListViewModel>> GetPlansForDropDownAsync(CancellationToken ct = default)
        {
            var allPlans = await unitOfWork.GetRepository<Plan>().GetAll(false, ct);
            var activePlans = allPlans.Where(p => p.IsActive);

            return mapper.Map<IEnumerable<PlanSelectListViewModel>>(activePlans);
        }

        public async Task<IEnumerable<MemberSelectListViewModel>> GetMembersForDropDownAsync(CancellationToken ct = default)
        {
            var members = await unitOfWork.GetRepository<Member>().GetAll(false, ct);

            return mapper.Map<IEnumerable<MemberSelectListViewModel>>(members);
        }
    }
}