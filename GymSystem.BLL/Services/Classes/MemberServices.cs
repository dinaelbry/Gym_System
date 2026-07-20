using GymSystem.BLL.Services.Interfaces;
using GymSystem.BLL.ViewModels.MembersViewModels;
using GymSystem.DAL.Data.Entities;
using GymSystem.DAL.Repository.Classes;
using GymSystem.DAL.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.BLL.Services.Classes
{
    public class MemberServices : IMemberServices
    {
        private readonly IGenericRepository<Member> memberRepository;
        private readonly IGenericRepository<MemberShip> membershipRepository;
        private readonly IGenericRepository<Plan> planRepository;
        private readonly IGenericRepository<HealthRecord> healthRecordRepository;
        private readonly IGenericRepository<Booking> bookingRepository;
        public MemberServices(IGenericRepository<Member> memberRepository, IGenericRepository<MemberShip> membershipRepository, IGenericRepository<Plan> planRepository, IGenericRepository<HealthRecord> healthRecordRepository,IGenericRepository<Booking> bookingRepository)
        {
            this.memberRepository = memberRepository;
            this.membershipRepository = membershipRepository;
            this.planRepository = planRepository;
            this.healthRecordRepository = healthRecordRepository;
            this.bookingRepository = bookingRepository;
        }

        //get
        public async Task<IEnumerable<MemberViewModel>> GetAllMembersAsync(CancellationToken ct = default)
        {
            var members = await memberRepository.GetAll(false, ct);
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
            var member = await memberRepository.GetById(memberId, ct);
            if (member == null) return null;

            var memberVM = new MemberViewModel()
            {
                Name = member.Name,
                Email = member.Email,
                Phone = member.PhoneNumber,
                BirthDate = member.BirthDate.ToShortDateString(),
                Gender = member.Gender.ToString(),
                Address = $"{member.Address.BuildingNumber} - {member.Address.Street} - {member.Address.City}"
            };
                  
                //membership details                
                var ActiveMembership = await membershipRepository.FirstOrDefaultAsync(mb => mb.MemberId == memberId && mb.EndDate > DateTime.Now,false, ct);

            if (ActiveMembership is not null)
            {
                var ActivePlan = await planRepository.GetById(ActiveMembership.PlanId, ct);
                memberVM.PlanName = ActivePlan?.Name;
                memberVM.MembershipStartDate = ActiveMembership.CreatedAt.ToShortDateString();
               memberVM.MembershipEndDate = ActiveMembership.EndDate.ToShortDateString();

            }

            return memberVM;
        } 


        public async Task<HealthRecordViewModel?> GetMemberHealthRecordAsync(int memberId, CancellationToken ct = default)
        {
            var Record = await healthRecordRepository.FirstOrDefaultAsync(r => r.MemberId == memberId, false, ct);
            if (Record is null) return null;
            return new HealthRecordViewModel ()
            {
                Height = Record.Height,
                Weight = Record.Weight,
                BloodType = Record.BloodType,
                Note = Record.Note
            };
        }

        public async Task<MemberToUpdateViewModel?> GetMemberToUpdateAsync(int memberId, CancellationToken ct = default)
        {
            var member = await memberRepository.GetById(memberId, ct);
            if (member is null) return null;
            return new MemberToUpdateViewModel()
            {
                Name = member.Name,
                Email = member.Email,
                Phone = member.PhoneNumber,
                Photo = member.Photo,
                BuildingNumber = member.Address.BuildingNumber,
                Street = member.Address.Street,
                City = member.Address.City
            };
        }

        //post

        public async Task<bool> CreateMemberAsync(CreateMemberViewModels model, CancellationToken ct = default)
        {
            // GET ALL 
            // Any (Expression <Func<TEntity, bool>> predicate)
            var EmailExists = await memberRepository.AnyAsync(m => m.Email == model.Email, ct);
            var PhoneExists = await memberRepository.AnyAsync(m => m.PhoneNumber == model.PhoneNumber, ct);

            if (EmailExists || PhoneExists) return false;

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
            var result  = await memberRepository.Add(member,ct);
            return result > 0 ? true : false;
        }

        public async Task<bool> UpdateMemberDetailsAsync(int id, MemberToUpdateViewModel model, CancellationToken ct = default)
        {
            var member = await memberRepository.GetById(id, ct);
            if (member is null) return false;
            if (await memberRepository.AnyAsync(m=> m.Email == model.Email && m.Id != id, ct)) return false;
            if (await memberRepository.AnyAsync(m => m.PhoneNumber == model.Phone && m.Id != id, ct)) return false;
            member.Email= model.Email;
            member.PhoneNumber = model.Phone;
            member.Address.City = model.City;
            member.Address.Street = model.Street;
            member.Address.BuildingNumber = model.BuildingNumber;
            member.UpdatedAt= DateTime.Now;

           var result = await memberRepository.Update(member,ct);
            return result > 0 ? true : false;

        }
        public async Task<bool> DeleteMemberAsync(int memberId,CancellationToken ct = default)
        {
            var hasFutureSessions = await bookingRepository.AnyAsync(b => b.MemberId == memberId && b.Session.EndDate > DateTime.Now,ct);

            if (hasFutureSessions) return false;

            var member = await memberRepository.GetById(memberId, ct);

            if (member is null)  return false;

            var result = await memberRepository.Delete(member, ct);

            return result > 0 ? true : false;
        }


    }
}
