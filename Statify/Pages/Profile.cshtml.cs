using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Statify.Interfaces;
using Statify.Models;

namespace Statify.Pages
{
    public class ProfileModel : PageModel
    {
        private readonly IUserService _userService;
        [BindProperty]
        public new User? User { get; set; }
        public PlayListCollection? Playlists { get; set; }

        public ProfileModel(IUserService userService)
        {
            _userService = userService;
        }

        public async Task OnGetAsync()
        {
            Playlists = new PlayListCollection();
            User = new();
            User = await _userService.GetUserFromSpotifyWithWebApi();
            Playlists = await _userService.GetPlaylists();

        }
    }
}
