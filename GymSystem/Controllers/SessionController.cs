using GymSystem.BLL.Services.Interfaces;
using GymSystem.BLL.ViewModels.SessionViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace GymSystem.Controllers
{
    public class SessionController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
