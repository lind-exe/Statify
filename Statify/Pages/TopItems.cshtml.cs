using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Statify.Interfaces;
using Statify.Models;
using Statify.Services;

namespace Statify.Pages
{
    public class TopItemsModel : PageModel
    {
        private readonly ISpotifyService _spotifyService;
        private readonly IUserService _userService;
        public TrackCollection TrackCollection { get; set; }
        public ArtistCollection ArtistCollection { get; set; }


        public TopItemsModel(ISpotifyService spotifyService, IUserService userService)
        {
            _spotifyService = spotifyService;
            _userService = userService;
        }

        public async Task OnGetAsync(string itemType, string term)
        {
            if (!string.IsNullOrEmpty(itemType) && !string.IsNullOrEmpty(term))
            {
                if (itemType == "tracks")
                {

                    TrackCollection = await _userService.GetTopItems<TrackCollection>(itemType, term);
                    
                }
                else if (itemType == "artists")
                {
                    ArtistCollection = await _userService.GetTopItems<ArtistCollection>(itemType, term);
                }
            }
            else
            {
                // Handle the case where either itemType or term is not selected
            }
        }
    }
}
