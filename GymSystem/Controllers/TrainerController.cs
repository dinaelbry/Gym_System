using Microsoft.AspNetCore.Mvc;

namespace GymSystem.Controllers
{
    public class TrainerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
