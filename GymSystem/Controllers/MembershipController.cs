using GymSystem.DAL.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading;

namespace GymSystem.Controllers
{
    public class MembershipController : Controller
    {
        private readonly IUnitOfWork unitOfWork;

        public MembershipController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }



       
    }
}
