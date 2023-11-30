using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Statify.Interfaces;
using Statify.Models;
using Statify.Services;

namespace Statify.Pages
{
    public class StatisticsModel : PageModel
    {
        private readonly IStatisticsService _statisticsService;
        public Dictionary<string, int>? Genres { get; set; }        

        public StatisticsModel(IStatisticsService statisticsService)
        {
            _statisticsService = statisticsService;
        }

        public async Task OnGetAsync()
        {

            Genres = await _statisticsService.ToBeDecided();
        }
    }
}
