using AutoMapper;
using GymSystem.BLL.Common;
using GymSystem.BLL.Services.Interfaces;
using GymSystem.BLL.ViewModels.PlanViewModels;
using GymSystem.DAL.Entities;
using GymSystem.DAL.Repository.Interfaces;

namespace GymSystem.BLL.Services.Classes
{
    public class PlanService(IUnitOfWork unitOfWork, IMapper mapper) : IPlanService
    {
        public async Task<IEnumerable<PlanViewModel>> GetAllPlansAsync(CancellationToken ct = default)
        {
            var repo = unitOfWork.GetRepository<Plan>();
            var plans = await repo.GetAll(false,ct);

            return mapper.Map<IEnumerable<PlanViewModel>>(plans);
        }
        public async Task<PlanViewModel?> GetPlanByIdAsync(int planId, CancellationToken ct = default)
        {
            var repo = unitOfWork.GetRepository<Plan>();
            var plan = await repo.GetById(planId, ct);
            if (plan is null) return null;

            return mapper.Map<PlanViewModel>(plan);
        }

        public async Task<UpdatePlanViewModel?> GetPlanToUpdateAsync(int planId, CancellationToken ct = default)
        {
            var repo = unitOfWork.GetRepository<Plan>();
            var plan = await repo.GetById(planId, ct);
            if (plan is null || !plan.IsActive) return null;
            if (await HasActiveMembershipsAsync(planId, ct)) return null;

            return mapper.Map<UpdatePlanViewModel>(plan);
        }

        public async Task<Result> ToggleActivationAsync(int planId, CancellationToken ct = default)
        {
            var repo = unitOfWork.GetRepository<Plan>();
            var plan = await repo.GetById(planId, ct);
            if (plan is null) return Result.NotFound("Plan not found.");

            if (plan.IsActive && await HasActiveMembershipsAsync(planId, ct))
                return Result.Fail("Cannot deactivate a plan that has active memberships.");

            plan.IsActive = !plan.IsActive;
            plan.UpdatedAt = DateTime.Now;

            repo.Update(plan, ct);
            var result = await unitOfWork.CompleteAsync(ct);

            return result > 0 ? Result.Ok() : Result.Fail("Failed to Toggle Plan Status");
        }
        public async Task<Result> UpdatePlanAsync(int id, UpdatePlanViewModel model, CancellationToken ct = default)
        {
            var repo = unitOfWork.GetRepository<Plan>();
            var plan = await repo.GetById(id, ct);
            if (plan is null) return Result.NotFound("Plan not found.");
      if (await HasActiveMembershipsAsync(id, ct))
                return Result.Fail("Cannot edit a plan that has active memberships.");

            mapper.Map(model, plan);


            repo.Update(plan,ct);
            var result = await unitOfWork.CompleteAsync(ct);

            return result > 0 ? Result.Ok() : Result.Fail("Failed to update plan.");
        }


        private async Task<bool> HasActiveMembershipsAsync(int planId, CancellationToken ct)
        {
            return await unitOfWork.GetRepository<MemberShip>()
                .Any(m => m.PlanId == planId && m.EndDate > DateTime.Now, ct);
        }

        
    }
}