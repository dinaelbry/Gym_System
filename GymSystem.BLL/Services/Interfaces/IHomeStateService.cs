
using GymSystem.BLL.ViewModels.HomeViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.BLL.Services.Interfaces
{
    public interface IHomeStateService
    {
        Task<HomeStatsViewModel> GetStatesDataAsync(CancellationToken ct = default);
    }
}
