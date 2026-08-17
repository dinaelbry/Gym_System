using AutoMapper;
using GymSystem.BLL.ViewModels.BookingViewModels;
using GymSystem.BLL.ViewModels.MembershipViewModels;
using GymSystem.BLL.ViewModels.MembersViewModels;
using GymSystem.BLL.ViewModels.PlanViewModels;
using GymSystem.BLL.ViewModels.SessionViewModels;
using GymSystem.BLL.ViewModels.TrainerViewModel;
using GymSystem.DAL.Entities;

namespace GymSystem.BLL.Utilites
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            MapTrainer();
            MapMember();
            MapPlan();
            MapSession();
            MapMemberships();
            MapBooking();


        }

        private void MapTrainer()
        {
            CreateMap<CreateTrainerViewModel, Trainer>()
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => new Address
                {
                    BuildingNumber = src.BuildingNumber,
                    Street = src.Street,
                    City = src.City

                }))
                .ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src => src.DateOfBirth))
                .ForMember(dest => dest.Specialize, opt => opt.MapFrom(src => src.Specialties))
                .ForMember(dest => dest.HireingDate, opt => opt.MapFrom(src => DateTime.Now));



            CreateMap<Trainer, TrainerViewModel>()
                .ForMember(dest => dest.Photo, opt => opt.Ignore())
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => $"{src.Address.BuildingNumber} - {src.Address.Street} - {src.Address.City}"))
                .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.BirthDate.ToShortDateString()))
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender.ToString()))
                .ForMember(dest => dest.Specialization, opt => opt.MapFrom(src => src.Specialize.ToString()));


            CreateMap<Trainer, TrainerToUpdateViewModel>()
                .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.PhoneNumber))
                .ForMember(dest => dest.Specialties, opt => opt.MapFrom(src => src.Specialize))
                .ForMember(dest => dest.Street, opt => opt.MapFrom(src => src.Address.Street))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.Address.City))
                .ForMember(dest => dest.BuildingNumber, opt => opt.MapFrom(src => src.Address.BuildingNumber));


            CreateMap<TrainerToUpdateViewModel, Trainer>()
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.Phone))
                .ForMember(dest => dest.Specialize, opt => opt.MapFrom(src => src.Specialties))
                .ForMember(dest => dest.Name, opt => opt.Ignore())
                .AfterMap((src, dest) =>
                {
                    dest.Address.BuildingNumber = src.BuildingNumber;
                    dest.Address.City = src.City;
                    dest.Address.Street = src.Street;
                    dest.UpdatedAt = DateTime.Now;
                });


        }

        private void MapMember()
        {
            CreateMap<CreateMemberViewModels, Member>()
                 .ForMember(dest => dest.Address, opt => opt.MapFrom(src => new Address
                 {
                     BuildingNumber = src.BuildingNumber,
                     City = src.City,
                     Street = src.Street
                 }))
                 .ForMember(dest => dest.HealthRecord, opt => opt.MapFrom(src => src.HealthRecordViewModel));

            CreateMap<HealthRecordViewModel, HealthRecord>().ReverseMap();

            CreateMap<Member, MemberViewModel>()
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender.ToString()))
                .ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src => src.BirthDate.ToShortDateString()))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => $"{src.Address.BuildingNumber} - {src.Address.Street} - {src.Address.City}"))
                .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.PhoneNumber));

            CreateMap<Member, MemberToUpdateViewModel>()
                .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.PhoneNumber))
                .ForMember(dest => dest.PhotoName, opt => opt.MapFrom(src => src.Photo))
                .ForMember(dest => dest.BuildingNumber, opt => opt.MapFrom(src => src.Address.BuildingNumber))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.Address.City))
                .ForMember(dest => dest.Street, opt => opt.MapFrom(src => src.Address.Street));

            CreateMap<MemberToUpdateViewModel, Member>()
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.Phone))
                .ForMember(dest => dest.Name, opt => opt.Ignore())
                .ForMember(dest => dest.Photo, opt => opt.Ignore())
                .AfterMap((src, dest) =>
                {
                dest.Address.BuildingNumber = src.BuildingNumber;
                dest.Address.City = src.City;
                dest.Address.Street = src.Street;
                dest.UpdatedAt = DateTime.Now;
            });
        }

        private void MapPlan()
        {
            CreateMap<Plan, PlanViewModel>();
            CreateMap<Plan, UpdatePlanViewModel>().ForMember(dest => dest.PlanName, opt => opt.MapFrom(src => src.Name));

            CreateMap<UpdatePlanViewModel, Plan>()
             .ForMember(dest => dest.Duration, opt => opt.MapFrom(src => src.DurationDays))
             .ForMember(dest => dest.Name, opt => opt.Ignore())
             .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.Now));
        }

        private void MapSession()
        {
            CreateMap<CreateSessionViewModel, Session>();
            CreateMap<Session, SessionViewModel>()
                .ForMember(dest=> dest.TrainerName, opt => opt.MapFrom(src => src.Trainer.Name))
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.CategoryName))
                .ForMember(dest => dest.AvailableSlots, opt => opt.Ignore());


            CreateMap<Session, UpdateSessionViewModel>();
            CreateMap<UpdateSessionViewModel, Session>();

            CreateMap<Trainer, TrainerSelectViewModel>();
            CreateMap<Category, CategorySelectViewModel>();
        }


        private void MapMemberships()
        {
            CreateMap<MemberShip, MembershipForMemberViewModel>()
                    .ForMember(dist => dist.MemberName, Option => Option.MapFrom(Src => Src.Member.Name))
                    .ForMember(dist => dist.PlanName, Option => Option.MapFrom(Src => Src.Plan.Name))
                    .ForMember(dist => dist.StartDate, Option => Option.MapFrom(X => X.CreatedAt));

            CreateMap<MemberShip, MembershipViewModel>()
                     .ForMember(dist => dist.MemberName, Option => Option.MapFrom(Src => Src.Member.Name))
                     .ForMember(dist => dist.PlanName, Option => Option.MapFrom(Src => Src.Plan.Name))
                     .ForMember(dist => dist.StartDate, Option => Option.MapFrom(X => X.CreatedAt));

            CreateMap<CreateMembershipViewModel, MemberShip>();
            CreateMap<Member, MemberSelectListViewModel>();
            CreateMap<Plan, PlanSelectListViewModel>();
        }

        private void MapBooking()
        {
            CreateMap<CreateBookingViewModel, Booking>()
                .AfterMap((src, dest) =>
                {
                    dest.IsAttended = false;
                    dest.CreatedAt = DateTime.Now;
                });

            CreateMap<Booking, MemberForSessionViewModel>()
                .ForMember(dest => dest.MemberName, opt => opt.MapFrom(src => src.Member.Name))
                .ForMember(dest => dest.BookingDate, opt => opt.MapFrom(src => src.Member.CreatedAt.ToString("yyyy-MM-dd HH:mm")));


        }


    }
}