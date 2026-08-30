using GymSystem.BLL.Services.Interfaces;
using GymSystem.DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;



namespace GymSystem.Controllers
{
    [Authorize(Roles = "Member")]

    public class MyAccountController : Controller
    {

        private readonly IMemberServices memberServices;
        private readonly UserManager<ApplicationUser> userManager;

        public MyAccountController(IMemberServices memberServices, UserManager<ApplicationUser> userManager)
        {
            this.memberServices = memberServices;
            this.userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index(CancellationToken ct)
        {
            //access for who actully login now 
            var currentUser = await userManager.GetUserAsync(User); 
            if (currentUser?.MemberId is null)
            {
                TempData["ErrorMessage"] = "No member account linked to this login.";
                return RedirectToAction("Login", "Account");
            }

            var model = await memberServices.GetMyAccountAsync(currentUser.MemberId.Value, ct);
            if (model is null)
            {
                TempData["ErrorMessage"] = "Member data not found.";
                return RedirectToAction("Login", "Account");
            }

            return View(model);
        }
    }
}