using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVC_Sessions.Contexts;

namespace MVC_Sessions.Controllers
{
    public class PlanController : Controller
    {
        // Session 1 MVC Assignment
        // We Have Two Actions : Index ==> For All Plans , Details ==> For Spcific Plan() ** By ID
        private readonly GymDbContext _dbContext = new GymDbContext();
        public async Task<IActionResult> Index()
        {
            var plans = await _dbContext.Plans.ToListAsync();
            return View(plans);
        }

        public async Task<IActionResult> Details(int id)
        {
            var plan = await _dbContext.Plans.FindAsync(id);
            if (plan == null)
            {
                RedirectToAction(nameof(Index)); // Tepm Until Make Action Not Found .
            }
            return View(plan);
        }

    }
}
