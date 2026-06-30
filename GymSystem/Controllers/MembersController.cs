using GymManagementSystem.BLL.Services.Interfaces;
using GymManagementSystem.BLL.ViewModels.MemberViewModels;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementSystem.PL.Controllers
{
    public class MembersController : Controller
    {
        private readonly IMemberService _memberService;

        public MembersController(IMemberService memberService)
        {
            _memberService = memberService;
        }

        // GET BaseUrl/Members/Index
        // Index - List all members


        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var members = await _memberService.GetAllMemberAsync(ct);
            return View(members);
        }

        // GET BaseUrl/Members/MemberDetails/{id}
        // MemberDetails - Show one member's details 

        public async Task<IActionResult> MemberDetails(int id, CancellationToken ct)
        {
            // Get Member By Id 
            var member = await _memberService.GetMemberDetailsByIdAsync(id, ct);
            if (member is null)
            {
                TempData["ErrorMessage"] = "Member Not Found";
                return RedirectToAction(nameof(Index));
            }
            return View(member);
        }


        // GET BaseUrl/Members/HealthRecordDetails/{id}
        // HealthRecordDetails - Show one member's details 

        public async Task<IActionResult> HealthRecordDetails(int id, CancellationToken ct)
        {
            // Get Health Record By Member Id 
            var result = await _memberService.GetMemberHealthRecordAsync(id, ct);
            if (result is null)
            {
                TempData["ErrorMessage"] = "Health Record Not Found";
                return RedirectToAction(nameof(Index));
            }
            return View(result);
        }



        #region Create Member

        // Get  BaseUrl/Members/Create
        // Create -Show empty form
        [HttpGet]
        public IActionResult Create() => View();


        // POST BaseUrl/Members/Create {Member}
        // CreateMember - Submit Form 
        [HttpPost]
        public async Task<IActionResult> CreateMember(CreateMemberViewModel model, CancellationToken ct)
        {
            if (!ModelState.IsValid) return View(nameof(Create), model);

            var result = await _memberService.CreateMemberAsync(model, ct);
            if (result)
                TempData["SuccessMessage"] = "Member Created Successfully";
            else
                TempData["ErrorMessage"] = "Failed To Create Member";

            return RedirectToAction(nameof(Index));

        }

        #endregion

        #region Edit Member 
        // Get  BaseUrl/Members/Edit/{id}
        // Edit  - Displays edit form
        [HttpGet]
        public async Task<IActionResult> EditMember(int id, CancellationToken ct)
        {
            var member = await _memberService.GetMemberToUpdateAsync(id, ct);
            if (member == null)
            {
                TempData["ErrorMessage"] = "Member Is Not Found";
                return RedirectToAction(nameof(Index));
            }
            return View(member);
        }

        // POST BaseUrl/Members/Edit {Member}
        // Edit - Submit Form 
        [HttpPost]
        public async Task<IActionResult> EditMember([FromRoute] int id, MemberToUpdateViewModel model, CancellationToken ct)
        {
            if (!ModelState.IsValid) return View(model);

            var result = await _memberService.UpdateMemberDetailsAsync(id, model, ct);
            if (result)
                TempData["SuccessMessage"] = "Member Update Successfully";
            else
                TempData["ErrorMessage"] = "Failed To Update Member";

            return RedirectToAction(nameof(Index));

        }
        #endregion

        #region Delete Member 
        // Get  BaseUrl/Members/Delete/{id}
        // Delete  -  Show Form 

        [HttpGet]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            var member = await _memberService.GetMemberDetailsByIdAsync(id, ct);
            if (member == null)
            {
                TempData["ErrorMessage"] = "Member Not Found";
                return RedirectToAction(nameof(Index));
            }
            return View();
        }


        // POST BaseUrl/Members/DeleteConfirmed/{id}
        // DeleteConfirmed - Submit Form 
        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed([FromRoute] int id, CancellationToken ct)
        {
            var result = await _memberService.RemoveMemberAsync(id, ct);
            if (result)
                TempData["SuccessMessage"] = "Member Deleted Successfully";
            else
                TempData["ErrorMessage"] = "Failed To delete Member";

            return RedirectToAction(nameof(Index));
        }
        #endregion
    }
}
