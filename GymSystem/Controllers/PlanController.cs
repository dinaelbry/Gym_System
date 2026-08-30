using GymSystem.BLL.Services.Interfaces;
using GymSystem.BLL.ViewModels.PlanViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;



namespace GymSystem.Controllers
{
    [Authorize(Roles = "SuperAdmin,Admin,Receptionist")]
    public class PlanController : Controller
    {
        private readonly IPlanService planService;

        public PlanController(IPlanService planService)
        {
            this.planService = planService;
        }

        public async Task<IActionResult> Index(CancellationToken ct) {
            var plans = await planService.GetAllPlansAsync(ct);
            return View(plans);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id, CancellationToken ct)
        {
            var plan = await planService.GetPlanByIdAsync(id, ct);
            if (plan is null) return RedirectToAction(nameof(Index));

            return View(plan);
        }

        [Authorize(Roles = "SuperAdmin,Admin")]
        [HttpGet]
        public async Task<IActionResult> Edit(int id, CancellationToken ct)
        {
            var plan = await planService.GetPlanToUpdateAsync(id, ct);
            if (plan is null)
            {
                TempData["ErrorMessage"] = "Plan cannot be edited (not found, inactive, or has active memberships).";
                return RedirectToAction(nameof(Index));
            }

            return View(plan);
        }


        [Authorize(Roles = "SuperAdmin,Admin")]
        [HttpPost]
        public async Task<IActionResult> Edit(int id, UpdatePlanViewModel model, CancellationToken ct)
        {
            if (!ModelState.IsValid) return View(model);

            var result = await planService.UpdatePlanAsync(id, model, ct);
            if (result.Success)
            {
                TempData["SuccessMessage"] = "Plan updated successfully.";
                return RedirectToAction(nameof(Index));
            }
            TempData["ErrorMessage"] = result.Error;
            return View(model);
        }


        [Authorize(Roles = "SuperAdmin,Admin")]
        [HttpPost]
        public async Task<IActionResult> ToggleActive(int id, CancellationToken ct)
        {
            var result = await planService.ToggleActivationAsync(id, ct);
            TempData[result.Success ? "SuccessMessage" : "ErrorMessage"] =
                result.Success ? "Plan status changed." : result.Error;
            return RedirectToAction(nameof(Index));
        }
    }
}
