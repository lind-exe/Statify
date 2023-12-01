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
        public Track Track { get; set; }
        public TrackCollection LikedSongs { get; set; }
        public TrackCollection SpecifiedTracks { get; set; }
        public StatisticsModel(IStatisticsService statisticsService)
        {
            _statisticsService = statisticsService;
        }

        public async Task OnGetAsync()
        {
            Track = await _statisticsService.GetTrack("11dFghVXANMlKmJXsNCbNl");
            LikedSongs = await _statisticsService.GetLikedSongs(50, 0);  // Does not deserialize properly but response and content is correct
            SpecifiedTracks = await _statisticsService.GetSeveralTracks("11dFghVXANMlKmJXsNCbNl%11dFghVXANMlKmJXsNCbNl"); // does not return anything.

            Genres = await _statisticsService.GetCalculatedGenreData();

        }
    }
}
