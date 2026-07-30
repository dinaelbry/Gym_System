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
        [HttpGet]
        public IActionResult Create() => View();

        [HttpPost] 
        public async Task<IActionResult> CreateMember(CreateMemberViewModels model, CancellationToken ct)
        {
            if (!ModelState.IsValid)
                return View(nameof(Create), model);

           var result=  await _memberServices.CreateMemberAsync(model, ct);
            if (result)
            {
                TempData["Success"] = "Member created successfully!";
            }
            else
            {
                TempData["Failed"] = "Failed to create member.";
            }
            return RedirectToAction(nameof(Index));
            
            
        }

        [HttpGet]
        public async Task<IActionResult> EditMember(int id, CancellationToken ct)
        {
            var member = await _memberServices.GetMemberToUpdateAsync(id, ct);
            if (member == null)
            {
                TempData["ErrorMEssage"] = "Member Not Found";
                return RedirectToAction(nameof(Index));
            }
            return View(member);
        }

        [HttpPost]
        public async Task<IActionResult> EditMember([FromRoute]int id, MemberToUpdateViewModel model, CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await _memberServices.UpdateMemberDetailsAsync(id, model, ct);
            if (result)
            {
                TempData["Success"] = "Member updated successfully";
            }
            else
            {
                TempData["Failed"] = "Failed To update member";
            }
            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        public async Task<IActionResult> MemberDetails(int id, CancellationToken ct)
        {
         
            var member = await _memberServices.GetMemberDetailsAsync(id, ct);
            if (member is null)
            {
                TempData["ErrorMessage"] = "Member Not Found!";
                return RedirectToAction(nameof(Index));
            }
            
            return View(member);
        }
        [HttpGet]
        public async Task<IActionResult> HealthRecordDetails(int id, CancellationToken ct)
        {
            var healthrecord = await _memberServices.GetMemberHealthRecordAsync(id, ct);
            if (healthrecord == null)
            {
                TempData["ErrorMessage"] = "Health Record Not Found!";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(healthrecord);

            }

        }

        [HttpGet]
        public async Task<IActionResult> DeleteMember(int id, CancellationToken ct)
        {
            var member = await _memberServices.GetMemberDetailsAsync(id, ct);
            if (member is null)
            {
                TempData["ErrorMessage"] = "Member Not Found";
                return RedirectToAction(nameof(Index));
            }
            return View(member);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id, CancellationToken ct) 
        {
            var result = await _memberServices.DeleteMemberAsync(id, ct);
            if (result)
            {
                TempData["Success"]  = "Member deleted successfully!";
            }
            else
            {
                TempData["Failed"] = "Failed to delete member. Member may have upcoming sessions.";
            }
            return RedirectToAction(nameof(Index));

        }


   }

}
