using GymSystem.BLL.Services.Attachment;
using GymSystem.BLL.Services.Classes;
using GymSystem.BLL.Services.Interfaces;
using GymSystem.BLL.ViewModels.MembersViewModels;
using GymSystem.DAL.Entities;
using GymSystem.BLL.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymSystem.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    public class MemberController(IMemberServices memberServices, IAttachmentService attachmentService) : Controller
    {
        private readonly IMemberServices _memberServices = memberServices;
        private readonly IAttachmentService _attachmentService = attachmentService;

        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var members = await _memberServices.GetAllMembersAsync(ct);
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
                TempData["SuccessMessage"] = "Member created successfully.";
                return RedirectToAction(nameof(Index));
            }

            TempData["ErrorMessage"] = "Failed to create member. Email or phone number may already be in use.";
            return View(nameof(Create), model);

        }

        public async Task<IActionResult> Picture(int id)
        {
            var member = await _memberServices.GetMemberDetailsAsync(id);
            if (member is null || string.IsNullOrEmpty(member.Photo))
                return NotFound();


            var result = _attachmentService.GetFile(member.Photo, "images");
            if (result is null) return NotFound();

            return File(result.Value.Stream, result.Value.ContentType);
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
                TempData["SuccessMessage"] = "Member updated successfully.";
                return RedirectToAction(nameof(Index));
            }

            TempData["ErrorMessage"] = "Failed to update member.";
            return View(model);
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
                TempData["ErrorMessage"] = "Failed to delete member. Member may have upcoming sessions.";
            }
            return RedirectToAction(nameof(Index));

        }


   }

}
