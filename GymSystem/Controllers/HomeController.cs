using GymSystem.BLL.Services.Interfaces;
using GymSystem.BLL.ViewModels.HomeViewModels;
using GymSystem.DAL.Entities;
using GymSystem.DAL.Repository.Classes;
using GymSystem.DAL.Repository.Interfaces;
using GymSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace GymSystem.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHomeStateService _homeStateService;
        private readonly ILogger<HomeController> _logger;
        public HomeController(ILogger<HomeController> logger, IHomeStateService homeStateService, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _homeStateService = homeStateService;
            _logger = logger;
        }


        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var Data = await _homeStateService.GetStatesDataAsync(ct);
            return View(Data);

        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });


        }
    }
}
