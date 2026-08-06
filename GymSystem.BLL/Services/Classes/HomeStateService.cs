using AutoMapper;
using GymSystem.BLL.Services.Interfaces;
using GymSystem.BLL.ViewModels.HomeViewModels;
using GymSystem.DAL.Entities;
using GymSystem.DAL.Repository.Interfaces;

namespace GymSystem.BLL.Services.Classes
{
    public class HomeStateService : IHomeStateService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public HomeStateService(IUnitOfWork unitOfWork,IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<HomeStatsViewModel> GetStatesDataAsync(CancellationToken ct = default)
        {
            var now = DateTime.Now;
            var upcomingSessions = await unitOfWork.GetRepository<Session>().CountAsync(s => s.StartDate > now);
            var ongoingSessions = await unitOfWork.GetRepository<Session>().CountAsync(X => X.StartDate <= now && X.EndDate >= now);
            var completedSessions = await unitOfWork.GetRepository<Session>().CountAsync(X => X.EndDate < now);
            var totalMembers = await unitOfWork.GetRepository<Member>().CountAsync(ct: ct);
            var totalTrainers = await unitOfWork.GetRepository<Trainer>().CountAsync(ct: ct);
            var activeMembers = await unitOfWork.GetRepository<MemberShip>().CountAsync(m => m.EndDate > now, ct);
            return new HomeStatsViewModel()
            {
                TotalMembers = totalMembers,
                TotalTrainers = totalTrainers,
                ActiveMembers = activeMembers,
                UpcomingSessions = upcomingSessions,
                OngoingSessions = ongoingSessions,
                CompletedSessions = completedSessions
            };
        }
    }
}