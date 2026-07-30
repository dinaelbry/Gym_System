using GymSystem.BLL.ViewModels.HomeViewModels;
using GymSystem.DAL.Data.Entities;
using GymSystem.DAL.Repository.Classes;
using GymSystem.DAL.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GymSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUnitOfWork UnitOfWork;

        public HomeController(IUnitOfWork UnitOfWork) 
        {
            this.UnitOfWork = UnitOfWork;
        }


        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var now = DateTime.Now;

            var members = await UnitOfWork.GetRepository<Member>().GetAll(false, ct);
            var trainers = await UnitOfWork.GetRepository<Trainer>().GetAll(false, ct);
            var memberShips = await UnitOfWork.GetRepository<MemberShip>().GetAll(false, ct);
            var sessions = await UnitOfWork.GetRepository<Session>().GetAll(false, ct);

            var model = new HomeStatsViewModel
            {
                TotalMembers = members.Count(),
                ActiveMembers = memberShips
                    .Where(ms => ms.EndDate > now)
                    .Select(ms => ms.MemberId)
                    .Distinct()
                    .Count(),
                TrainersCount = trainers.Count(),
                UpcomingSessions = sessions.Count(s => s.StartDate > now),
                OngoingSessions = sessions.Count(s => s.StartDate <= now && s.EndDate >= now),
                CompletedSessions = sessions.Count(s => s.EndDate < now)
            };
            return View(model);

        }

        public IActionResult Privacy()
        {
            return View();
        }

       
    }
}
