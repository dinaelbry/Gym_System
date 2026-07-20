using GymSystem.BLL.Services.Interfaces;
using GymSystem.BLL.ViewModels.MembersViewModels;
using Microsoft.AspNetCore.Mvc;

namespace GymSystem.Controllers
{
    public class MemberController : Controller
    {
        private readonly IMemberServices _memberServices;

        public MemberController(IMemberServices memberServices)
        {
            _memberServices = memberServices;
        }

        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            var members = await _memberServices.GetAllMembersAsync(cancellationToken);
            return View(members);
        }

        //Action
        public IActionResult Create() => View();
        public async Task<IActionResult> CreateMember(CreateMemberViewModels model, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return View(nameof(Create), model);

            var result = await _memberServices.CreateMemberAsync(model, cancellationToken);

            return RedirectToAction(nameof(Index));
            
            
        }

    }

}
