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

        public HomeStateService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<HomeStatsViewModel> GetStatesDataAsync(CancellationToken ct = default)
        {
            var now = DateTime.Now;
            var sessions = await unitOfWork.GetRepository<Session>().GetAll(true, ct);
            var totalMembers = await unitOfWork.GetRepository<Member>().CountAsync(ct: ct);
            var totalTrainers = await unitOfWork.GetRepository<Trainer>().CountAsync(ct: ct);
            var activeMembers = await unitOfWork.GetRepository<MemberShip>().CountAsync(m => m.EndDate > now, ct);           
           
            return new HomeStatsViewModel()
            {
                TotalMembers = totalMembers,
                TotalTrainers = totalTrainers,
                ActiveMembers = activeMembers,
                UpcomingSessions = sessions.Count(s => s.StartDate > now),
                OngoingSessions = sessions.Count(s => s.StartDate <= now && s.EndDate >= now),
                CompletedSessions = sessions.Count(s => s.EndDate < now)
            };
        }
    }
}