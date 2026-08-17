using AutoMapper;
using GymSystem.BLL.Services.Attachment;
using GymSystem.BLL.Services.Interfaces;
using GymSystem.BLL.ViewModels.MembersViewModels;
using GymSystem.DAL.Entities;
using GymSystem.DAL.Repository.Classes;
using GymSystem.DAL.Repository.Interfaces;


namespace GymSystem.BLL.Services.Classes
{
    public class MemberServices: IMemberServices
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IAttachmentService attachmentService ;
        private readonly IMapper mapper;

        public MemberServices(IUnitOfWork unitOfWork, IAttachmentService attachmentService, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.attachmentService = attachmentService;
            this.mapper = mapper;
        }

        //get
        public async Task<IEnumerable<MemberViewModel>> GetAllMembersAsync(CancellationToken ct = default)
        {
            var repo = unitOfWork.GetRepository<Member>();

            var members = await repo.GetAll(false, ct);
            if (!members.Any()) return [];

            return mapper.Map<IEnumerable<MemberViewModel>>(members);   
        }

        public async Task<MemberViewModel?> GetMemberDetailsAsync(int memberId, CancellationToken ct = default)
        {
            var repo = unitOfWork.GetRepository<Member>();

            // member + membership + plan
            var member = await repo.GetById(memberId, ct);
            if (member == null) return null;

            var memberVM = mapper.Map<MemberViewModel>(member);

            //membership details                
            var activeMembership = await unitOfWork.GetRepository<MemberShip>().FirstOrDefaultAsync(mb => mb.MemberId == memberId && mb.EndDate > DateTime.Now,false, ct);

            if (activeMembership is not null)
            {
                var activePlan = await unitOfWork.GetRepository<Plan>().GetById(activeMembership.PlanId, ct);
                memberVM.PlanName = activePlan?.Name;
                memberVM.MembershipStartDate = activeMembership.CreatedAt.ToShortDateString();
               memberVM.MembershipEndDate = activeMembership.EndDate.ToShortDateString();

            }

            return memberVM;
        } 


        public async Task<HealthRecordViewModel?> GetMemberHealthRecordAsync(int memberId, CancellationToken ct = default)
        {
            var record = await unitOfWork.GetRepository<HealthRecord>().FirstOrDefaultAsync(r => r.MemberId == memberId, false, ct);
            if (record is null) return null;
            return mapper.Map<HealthRecordViewModel>(record);
        }

        public async Task<MemberToUpdateViewModel?> GetMemberToUpdateAsync(int memberId, CancellationToken ct = default)
        {
            var repo = unitOfWork.GetRepository<Member>();

            var member = await repo.GetById(memberId, ct);
            if (member is null) return null;
            return mapper.Map<MemberToUpdateViewModel>(member);
        }

        //post

        public async Task<bool> CreateMemberAsync(CreateMemberViewModels model, CancellationToken ct = default)
        {
            var repo = unitOfWork.GetRepository<Member>();
            // GET ALL 
            // Any (Expression <Func<TEntity, bool>> predicate)
            var emailExists = await repo.Any(m => m.Email == model.Email, ct);
            var phoneExists = await repo.Any(m => m.PhoneNumber == model.PhoneNumber, ct);

            if (emailExists || phoneExists) return false;

            var member = mapper.Map<Member>(model);

            var NewPhotoName = await attachmentService.UploadAsync( model.Photo.OpenReadStream(),model.Photo.FileName, "images",ct);

            if (string.IsNullOrEmpty(NewPhotoName)) return false;
            member.Photo = NewPhotoName;


            repo.Add(member, ct);
            var result = await unitOfWork.CompleteAsync(ct);
            return result > 0 ;
        }

        public async Task<bool> UpdateMemberDetailsAsync(int id, MemberToUpdateViewModel model, CancellationToken ct = default)
        {
            var repo = unitOfWork.GetRepository<Member>();

            var member = await repo.GetById(id, ct);
            if (member is null) return false;
            if (await repo.Any(m=> m.Email == model.Email && m.Id != id, ct)) return false;
            if (await repo.Any(m => m.PhoneNumber == model.Phone && m.Id != id, ct)) return false;
            mapper.Map(model, member);
            member.Email = model.Email;

            if (model.Photo is not null)
            {
                if (!string.IsNullOrEmpty(member.Photo))
                {
                    attachmentService.Delete(member.Photo, "images");
                }
                var newPhotoName = await attachmentService.UploadAsync(
                    model.Photo.OpenReadStream(),
                    model.Photo.FileName,
                    "images",ct);

                member.Photo = newPhotoName;

            }

            repo.Update(member, ct);
            var result = await unitOfWork.CompleteAsync(ct);
            return result > 0;

        }
        public async Task<bool> DeleteMemberAsync(int memberId,CancellationToken ct = default)
        {
           var repo = unitOfWork.GetRepository<Member>();

            var member = await repo.GetById(memberId, ct);
            if (member is null) return false;

            var hasFutureSessions = await unitOfWork.GetRepository<Booking>().Any(b => b.MemberId == memberId && b.Session.EndDate > DateTime.Now,ct);
            if (hasFutureSessions) return false;


            if (member.Photo is not null)
                attachmentService.Delete(member.Photo, "images");


            repo.Delete(member, ct);
            var result = await unitOfWork.CompleteAsync(ct);

            if (result > 0 ){
                return true;
            }
            return false;
        }


    }
}
