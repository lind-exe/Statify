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
        public AudioFeature? AudioFeatures { get; set; }
        public string? UserTitle { get; set; }
        public TimeSpan? TotalDuration { get; set; }
        public StatisticsModel(IStatisticsService statisticsService)
        {
            _statisticsService = statisticsService;
        }
        public async Task OnGetAsync()
        {
            Genres = await _statisticsService.GetCalculatedGenreData();
            AudioFeatures = await _statisticsService.GetCalculatedAudioFeatures();
            UserTitle = await _statisticsService.GetUserSoundProfileTitle();
            TotalDuration = _statisticsService.GetTotalDurationOfTopSongs();
        }
    }
}
