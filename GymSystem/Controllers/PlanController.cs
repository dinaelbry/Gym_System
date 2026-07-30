using GymSystem.BLL.ViewModels.PlanViewModels;
using GymSystem.DAL.Data.Contexts;
using GymSystem.DAL.Data.Entities;
using GymSystem.DAL.Repository.Classes;
using GymSystem.DAL.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Numerics;
using System.Threading.Tasks;



namespace GymSystem.Controllers
{
    public class PlanController : Controller
    {
        private readonly IGenericRepository<Plan> planRepository;
        public PlanController(IGenericRepository<Plan> _planRepository)
        {
            planRepository = _planRepository;
        }

        public async Task<IActionResult> Index(CancellationToken token) {
            var plans = await planRepository.GetAll(false, token);
            return View(plans);
        }

        public async Task<IActionResult> Details(int id, CancellationToken token)
        {
            var plan = await planRepository.GetById(id, token);
            if (plan == null)

                return RedirectToAction(nameof(Index));

            return View(plan);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id, CancellationToken ct)
        {
            var plan = await planRepository.GetById(id, ct);
            if (plan is null)
            {
                return RedirectToAction(nameof(Index));
            }

            var model = new UpdatePlanViewModel
            {
                Id = plan.Id,
                PlanName = plan.Name!,
                Description = plan.Description!,
                DurationDays = plan.Duration,
                Price = plan.Price
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, UpdatePlanViewModel model, CancellationToken ct)
        {
            if (!ModelState.IsValid)
                return View(model);

            var plan = await planRepository.GetById(id, ct);
            if (plan is null)
                return RedirectToAction(nameof(Index));

            plan.Description = model.Description;
            plan.Duration = model.DurationDays;
            plan.Price = model.Price;
            plan.UpdatedAt = DateTime.Now;

             planRepository.Update(plan, ct);
            await planRepository.CompleteAsync(ct);

            return RedirectToAction(nameof(Index));
        }


        [HttpPost]
        public async Task<IActionResult> ToggleActive(int id, CancellationToken ct) {
            {
                var plan = await planRepository.GetById(id, ct);
                if (plan == null)
                {
                    return RedirectToAction(nameof(Index));
                }
                plan.IsActive = !plan.IsActive;
                plan.UpdatedAt = DateTime.Now;

                planRepository.Update(plan);
                await planRepository.CompleteAsync(ct);

                return RedirectToAction(nameof(Index));
            }

        }
    }
}
