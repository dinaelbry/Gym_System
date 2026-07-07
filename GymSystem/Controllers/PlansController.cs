using GymManagementSystem.DAL.Data.Configurations;
//using GymManagementSystem.BLL.ViewModels.PlanViewModels;
using GymManagementSystem.DAL.Data.Contexts;
using GymManagementSystem.DAL.Data.Entities;
using GymManagementSystem.DAL.Repositories.Classes;
using GymManagementSystem.DAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GymSystem.PL.Controllers
{
    public class PlansController : Controller
    {
        private readonly IGenericRepository<Plan> planRepository;

        public PlansController(IGenericRepository<Plan> planRepository)
        {
            this.planRepository = planRepository;
        }

        //get   Url: BaseUrl/Plans/Index
        public async Task<IActionResult> Index(CancellationToken token)
        {
            var plans = await planRepository.GetAllAsync(false, token) ;
            return View(plans);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id, CancellationToken token)
        {
            var plan = await planRepository.GetByIdAsync(id, token);
            if (plan is null)
            {
                //TempData["ErrorMessage"] = "Plan not found.";
                return RedirectToAction(nameof(Index));
            }
            return View(plan);
        }
        //[HttpGet]
        //public async Task<IActionResult> Edit(int id, CancellationToken ct)
        //{
        //    var plan = await _planService.GetPlanToUpdateAsync(id, ct);
        //    if (plan is null)
        //    {
        //        TempData["ErrorMessage"] = "Plan cannot be edited (not found, inactive, or has active memberships).";
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(plan);
        //}

        //[HttpPost]
        //public async Task<IActionResult> Edit(int id, UpdatePlanViewModel model, CancellationToken ct)
        //{
        //    if (!ModelState.IsValid) return View(model);

        //    var result = await _planService.UpdatePlanAsync(id, model, ct);
        //    if (result)
        //        TempData["SuccessMessage"] = "Plan updated successfully.";
        //    else
        //        TempData["ErrorMessage"] = "Plan Failed To update";
        //    return RedirectToAction(nameof(Index));
        //}

        //[HttpPost]
        //public async Task<IActionResult> Activate(int id, CancellationToken ct)
        //{
        //    var result = await _planService.ToggleActivationAsync(id, ct);
        //    if (result)
        //        TempData["SuccessMessage"] = "Plan status changed";
        //    else
        //        TempData["ErrorMessage"] = "Failed to Toggle Plan Status";
        //    return RedirectToAction(nameof(Index));
        //}
    }
}
