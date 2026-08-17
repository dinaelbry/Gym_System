using GymSystem.BLL.Services.Interfaces;
using GymSystem.BLL.ViewModels.TrainerViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymSystem.Controllers
{
    [Authorize]
    public class TrainerController(ITrainerService trainerService) : Controller
    {
        private readonly ITrainerService _trainerService = trainerService;

        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var trainers = await _trainerService.GetAllTrainersAsync(ct);
            return View(trainers);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "SuperAdmin")]
        [HttpPost]
        public async Task<IActionResult> Create(CreateTrainerViewModel model, CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await _trainerService.CreateTrainerAsync(model, ct);
            if (result.Success)
            {
                TempData["SuccessMessage"] = "Trainer created successfully.";
                return RedirectToAction(nameof(Index));
            }
            TempData["ErrorMessage"] = result.Error;
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id, CancellationToken ct)
        {
            var trainer = await _trainerService.GetTrainerDetailsAsync(id, ct);
            if (trainer is null)
            {
                TempData["ErrorMessage"] = "Trainer not found.";
                return RedirectToAction(nameof(Index));
            }
            return View(trainer);

        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id, CancellationToken ct)
        {
            var trainer = await _trainerService.GetTrainerToUpdateAsync(id, ct);
            if (trainer is null)
            {
                TempData["ErrorMessage"] = "Trainer not found.";
                return RedirectToAction(nameof(Index));
            }
            return View(trainer);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, TrainerToUpdateViewModel model, CancellationToken ct)
        {
            if (!ModelState.IsValid) return View(model);

            var result = await _trainerService.UpdateTrainerDetailsAsync(id, model, ct);
            if (result.Success)
            {
                TempData["SuccessMessage"] = "Trainer updated successfully.";
                return RedirectToAction(nameof(Index));
            }
            TempData["ErrorMessage"] = result.Error;
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            var trainer = await _trainerService.GetTrainerDetailsAsync(id, ct);
            if (trainer is null)
            {
                TempData["ErrorMessage"] = "Trainer not found.";
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id, CancellationToken ct)
        {
            var result = await _trainerService.DeleteTrainerAsync(id, ct);
            if (result.Success)
            {
                TempData["SuccessMessage"] = "Trainer deleted successfully.";
                return RedirectToAction(nameof(Index));
            }
            TempData["ErrorMessage"] = result.Error;
            return RedirectToAction(nameof(Index));
        }
    }
}
