using GymSystem.DAL.Data.Contexts;
using GymSystem.DAL.Repository.Interfaces;
using GymSystem.DAL.Repository.Classes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GymSystem.DAL.Data.Entities;



namespace GymSystem.Controllers
{
    public class PlanController : Controller
    {
        private readonly IGenericRepository<Plan> planRepository;
        public PlanController (IGenericRepository<Plan> _planRepository)
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

    }
}
