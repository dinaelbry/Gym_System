using AutoMapper;
using GymSystem.BLL.Common;
using GymSystem.BLL.Services.Interfaces;
using GymSystem.BLL.ViewModels.PlanViewModels;
using GymSystem.DAL.Entities;
using GymSystem.DAL.Repository.Interfaces;

namespace GymSystem.BLL.Services.Classes
{
    public class PlanService : IPlanService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper mapper;


        public PlanService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<PlanViewModel>> GetAllPlansAsync(CancellationToken ct = default)
        {
            var plans = await _unitOfWork.GetRepository<Plan>().GetAll(false,ct);


            return plans.Select(p => new PlanViewModel
            {
                Id = p.Id,
                Name = p.Name!,
                Description = p.Description!,
                Duration = p.Duration,
                Price = p.Price,
                IsActive = p.IsActive
            });
        }
        public async Task<PlanViewModel?> GetPlanByIdAsync(int planId, CancellationToken ct = default)
        {
            var plan = await _unitOfWork.GetRepository<Plan>().GetById(planId, ct);
            if (plan is null) return null;

            return new PlanViewModel
            {
                Id = plan.Id,
                Name = plan.Name!,
                Description = plan.Description!,
                Duration = plan.Duration,
                Price = plan.Price,
                IsActive = plan.IsActive
            };
        }

        public async Task<UpdatePlanViewModel?> GetPlanToUpdateAsync(int planId, CancellationToken ct = default)
        {
            var plan = await _unitOfWork.GetRepository<Plan>().GetById(planId, ct);
            if (plan is null || !plan.IsActive) return null;
            if (await HasActiveMembershipsAsync(planId, ct)) return null;

            return new UpdatePlanViewModel
            {
                Id = plan.Id,
                PlanName = plan.Name!,
                Description = plan.Description!,
                DurationDays = plan.Duration,
                Price = plan.Price
            };
        }

        public async Task<Result> ToggleActivationAsync(int planId, CancellationToken ct = default)
        {
            var repo = _unitOfWork.GetRepository<Plan>();
            var plan = await repo.GetById(planId, ct);
            if (plan is null) return Result.NotFound("Plan not found.");

            if (plan.IsActive && await HasActiveMembershipsAsync(planId, ct))
                return Result.Fail("Cannot deactivate a plan that has active memberships.");

            plan.IsActive = !plan.IsActive;
            plan.UpdatedAt = DateTime.Now;

            repo.Update(plan);
            var result = await _unitOfWork.CompleteAsync(ct);

            return result > 0 ? Result.Ok() : Result.Fail("Failed to Toggle Plan Status");
        }
        public async Task<Result> UpdatePlanAsync(int id, UpdatePlanViewModel model, CancellationToken ct = default)
        {
            var repo = _unitOfWork.GetRepository<Plan>();
            var plan = await repo.GetById(id, ct);
            if (plan is null) return Result.NotFound("Plan not found.");
      if (await HasActiveMembershipsAsync(id, ct))
                return Result.Fail("Cannot edit a plan that has active memberships.");

            plan.Description = model.Description;
            plan.Duration = model.DurationDays;
            plan.Price = model.Price;
            plan.UpdatedAt = DateTime.Now;

      
            repo.Update(plan,ct);
            var result = await _unitOfWork.CompleteAsync(ct);

            return result > 0 ? Result.Ok() : Result.Fail("Failed to update plan.");
        }


        private async Task<bool> HasActiveMembershipsAsync(int planId, CancellationToken ct)
        {
            return await _unitOfWork.GetRepository<MemberShip>()
                .Any(m => m.PlanId == planId && m.EndDate > DateTime.Now, ct);
        }

        
    }
}