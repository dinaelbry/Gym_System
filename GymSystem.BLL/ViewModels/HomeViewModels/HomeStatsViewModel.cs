using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.BLL.ViewModels.HomeViewModels
{
    public class HomeStatsViewModel
    {
        public int TotalMembers { get; set; }
        public int ActiveMembers { get; set; }
        public int UpcomingSessions { get; set; }
        public int OngoingSessions { get; set; }
        public int CompletedSessions { get; set; }
        public int TotalTrainers { get; set; }
    }
}
