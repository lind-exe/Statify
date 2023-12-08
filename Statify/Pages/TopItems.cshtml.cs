using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Statify.Interfaces;
using Statify.Models;
using Statify.Services;
using static Statify.Models.ArtistData;

namespace Statify.Pages
{
    public class TopItemsModel : PageModel
    {
        private readonly IUserService _userService;
        //public TrackCollection? TrackCollection { get; set; }
        public TrackData.TrackList? TrackCollection { get; set; }
        public ArtistItems? ArtistCollection { get; set; }
        public TopItemsModel(IUserService userService)
        {
            _userService = userService;
        }
        public async Task OnGetAsync(string itemType, string term)
        {
            if (!string.IsNullOrEmpty(itemType) && !string.IsNullOrEmpty(term))
            {
                if (itemType == "tracks")
                {
                    TrackCollection = await _userService.GetTopItems<TrackData.TrackList>(itemType, term);
                }
                else if (itemType == "artists")
                {
                    ArtistCollection = await _userService.GetTopItems<ArtistItems>(itemType, term);
                }
            }
        }
    }
}
