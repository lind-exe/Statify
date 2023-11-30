using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Statify.Interfaces;
using Statify.Models;
using Statify.Services;

namespace Statify.Pages
{
    public class StatisticsModel : PageModel
    {
        private readonly IUserService _userService;

        public TrackCollection? TrackCollection { get; set; }
        public ArtistCollection? ArtistCollection { get; set; }
        public AllGenres? AllGenres = new();

        public StatisticsModel(IUserService userService)
        {
            _userService = userService;
        }

        public async void OnGetAsync()
        {
            TrackCollection = await _userService.GetTopItems<TrackCollection>("tracks", "long_term", 50);
            ArtistCollection = await _userService.GetTopItems<ArtistCollection>("artists", "long_term", 50);
            
        }
    }
}
