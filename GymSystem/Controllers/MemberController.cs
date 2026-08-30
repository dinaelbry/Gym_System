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
    [Authorize(Roles = "SuperAdmin,Admin,Receptionist")]
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
        [Authorize(Roles = "SuperAdmin,Admin,Receptionist")]
        [HttpGet]
        public IActionResult Create() => View();


        [Authorize(Roles = "SuperAdmin,Admin,Receptionist")]
        [HttpPost] 
        public async Task<IActionResult> CreateMember(CreateMemberViewModels model, CancellationToken ct)
        {
            if (!ModelState.IsValid)
                return View(nameof(Create), model);

            var (success, temporaryPassword) =  await _memberServices.CreateMemberAsync(model, ct);
            if (success)
            {
                var confirmVm = new MemberCreatedViewModel
                {
                    Name = model.Name,
                    Email = model.Email,
                    TemporaryPassword = temporaryPassword!
                };
                return View("MemberCreated", confirmVm);
            }

            TempData["Failed"] = "Failed to create member. Email or phone number may already be in use.";
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


        [Authorize(Roles = "SuperAdmin,Admin")]
        [HttpGet]
        public async Task<IActionResult> EditMember(int id, CancellationToken ct)
        {
            var member = await _memberServices.GetMemberToUpdateAsync(id, ct);
            if (member == null)
            {
                TempData["Failed"] = "Member Not Found";
                return RedirectToAction(nameof(Index));
            }
            return View(member);
        }

        [Authorize(Roles = "SuperAdmin,Admin")]
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
                TempData["Success"] = "Member updated successfully.";
                return RedirectToAction(nameof(Index));
            }

            TempData["Failed"] = "Failed to update member.";
            return View(model);
        }


        [HttpGet]
        public async Task<IActionResult> MemberDetails(int id, CancellationToken ct)
        {
         
            var member = await _memberServices.GetMemberDetailsAsync(id, ct);
            if (member is null)
            {
                TempData["Failed"] = "Member Not Found!";
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
                TempData["Failed"] = "Health Record Not Found!";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(healthrecord);

            }

        }

        [Authorize(Roles = "SuperAdmin,Admin")]
        [HttpGet]
        public async Task<IActionResult> DeleteMember(int id, CancellationToken ct)
        {
            var member = await _memberServices.GetMemberDetailsAsync(id, ct);
            if (member is null)
            {
                TempData["Failed"] = "Member Not Found";
                return RedirectToAction(nameof(Index));
            }
            return View(member);
        }

        [Authorize(Roles = "SuperAdmin,Admin")]
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
