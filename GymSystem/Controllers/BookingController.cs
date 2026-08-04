using Microsoft.AspNetCore.Mvc;
using GymSystem.BLL.ViewModels.BookingViewModels;

namespace GymSystem.Controllers
{
    public class BookingController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
