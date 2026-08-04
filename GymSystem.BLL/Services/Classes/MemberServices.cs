using GymSystem.BLL.Services.Attachment;
using GymSystem.BLL.Services.Interfaces;
using GymSystem.BLL.ViewModels.MembersViewModels;
using GymSystem.DAL.Entities;
using GymSystem.DAL.Repository.Classes;
using GymSystem.DAL.Repository.Interfaces;


namespace GymSystem.BLL.Services.Classes
{
    public class MemberServices : IMemberServices
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IAttachmentService attachmentService;
        public MemberServices(IUnitOfWork unitOfWork, IAttachmentService attachmentService)
             
        {
            this.unitOfWork = unitOfWork;
            this.attachmentService = attachmentService;
        }

        //get
        public async Task<IEnumerable<MemberViewModel>> GetAllMembersAsync(CancellationToken ct = default)
        {
            var members = await unitOfWork.GetRepository<Member>().GetAll(false, ct);
            if (!members.Any()) return [];

            var MembersViewModel = members.Select(m => new MemberViewModel
            {
                Id = m.Id,
                Name = m.Name,
                Email = m.Email,
                Phone = m.PhoneNumber,
                Photo = m.Photo,
                Gender = m.Gender.ToString() 
            });

            return MembersViewModel;
            }

        public async Task<MemberViewModel?> GetMemberDetailsAsync(int memberId, CancellationToken ct = default)
        {
            // member + membership + plan
            var member = await unitOfWork.GetRepository<Member>().GetById(memberId, ct);
            if (member == null) return null;

            var memberVM = new MemberViewModel()
            {
                Id = member.Id,
                Name = member.Name,
                Email = member.Email,
                Phone = member.PhoneNumber,
                Photo = member.Photo,
                BirthDate = member.BirthDate.ToShortDateString(),
                Gender = member.Gender.ToString(),
                Address = member.Address != null
        ? $"{member.Address.BuildingNumber} - {member.Address.Street} - {member.Address.City}"
        : string.Empty
            };
                  
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
            return new HealthRecordViewModel ()
            {
                Height = record.Height,
                Weight = record.Weight,
                BloodType = record.BloodType,
                Note = record.Note
            };
        }

        public async Task<MemberToUpdateViewModel?> GetMemberToUpdateAsync(int memberId, CancellationToken ct = default)
        {
            var member = await unitOfWork.GetRepository<Member>().GetById(memberId, ct);
            if (member is null) return null;
            return new MemberToUpdateViewModel()
            {
                Name = member.Name,
                Email = member.Email,
                Phone = member.PhoneNumber,
                PhotoName = member.Photo,
                BuildingNumber = member.Address?.BuildingNumber ?? 0,
                Street = member.Address?.Street ?? string.Empty,
                City = member.Address?.City ?? string.Empty
            };
        }

        //post

        public async Task<bool> CreateMemberAsync(CreateMemberViewModels model, CancellationToken ct = default)
        {
            // GET ALL 
            // Any (Expression <Func<TEntity, bool>> predicate)
            var emailExists = await unitOfWork.GetRepository<Member>().Any(m => m.Email == model.Email, ct);
            var phoneExists = await unitOfWork.GetRepository<Member>().Any(m => m.PhoneNumber == model.PhoneNumber, ct);

            if (emailExists || phoneExists) return false;

            var member = new Member()
            {

                Name = model.Name,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                BirthDate = model.BirthDate.ToDateTime(TimeOnly.MinValue),
                Gender = model.Gender,
                Address = new Address()
                {
                    BuildingNumber = model.BuildingNumber,
                    Street = model.Street,
                    City = model.City
                },
                HealthRecord = new HealthRecord()
                {
                    Height = model.HealthRecordViewModel.Height,
                    Weight = model.HealthRecordViewModel.Weight,
                    BloodType = model.HealthRecordViewModel.BloodType,
                    Note = model.HealthRecordViewModel.Note
                }

            };
            // return bool 
            if (model.Photo is not null)
            {
                var photoName = await attachmentService.UploadAsync(
                    model.Photo.OpenReadStream(),
                    model.Photo.FileName,
                    "images",
                    ct);

                member.Photo = photoName;
            }
            unitOfWork.GetRepository<Member>().Add(member);
            var result = await unitOfWork.CompleteAsync();
            return result > 0 ;
        }

        public async Task<bool> UpdateMemberDetailsAsync(int id, MemberToUpdateViewModel model, CancellationToken ct = default)
        {
            var member = await unitOfWork.GetRepository<Member>().GetById(id, ct);
            if (member is null) return false;
            if (await unitOfWork.GetRepository<Member>().Any(m=> m.Email == model.Email && m.Id != id, ct)) return false;
            if (await unitOfWork.GetRepository<Member>().Any(m => m.PhoneNumber == model.Phone && m.Id != id, ct)) return false;
            member.Email= model.Email;
            member.PhoneNumber = model.Phone;
            member.Address ??= new Address();
            member.Address.City = model.City;
            member.Address.Street = model.Street;
            member.Address.BuildingNumber = model.BuildingNumber;

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
            member.UpdatedAt= DateTime.Now;

            unitOfWork.GetRepository<Member>().Update(member);
            var result = await unitOfWork.CompleteAsync();
            return result > 0 ?true:false ;

        }
        public async Task<bool> DeleteMemberAsync(int memberId,CancellationToken ct = default)
        {
            var hasFutureSessions = await unitOfWork.GetRepository<Booking>().Any(b => b.MemberId == memberId && b.Session.EndDate > DateTime.Now,ct);
            if (hasFutureSessions) return false;

            var member = await unitOfWork.GetRepository<Member>().GetById(memberId, ct);
            if (member is null) return false;

            if (!string.IsNullOrEmpty(member.Photo))
            {
                attachmentService.Delete(member.Photo, "images");
            }

             unitOfWork.GetRepository<Member>().Delete(member);
            var result = await unitOfWork.CompleteAsync();

            return result > 0;
        }


    }
}
