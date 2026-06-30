using GymSystem.DAL.Contexts;
using GymSystem.DAL.Repository.Interfaces;
using GymSystem.DAL.Repository.Classes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

    

namespace GymSystem.Controllers
{
    public class PlanController : Controller
    {
        private readonly IPlanRepository planRepository;
        public PlanController (IPlanRepository _planRepository)
        {
            planRepository = _planRepository;
        }

        public async Task<IActionResult> Index(CancellationToken token) { 
            var plans = await planRepository.GetAllPlans(false, token);
            return View(plans);
        }

        public async Task<IActionResult> Details(int id, CancellationToken token)
        {
            var plan = await planRepository.GetPlanById(id, token);
            if (plan == null)
            
              return RedirectToAction(nameof(Index)); 
            
            return View(plan);
        }

    }
}
