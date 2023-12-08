using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Statify.Interfaces;
using Statify.Models;

namespace Statify.Pages
{
    public class ProfileModel : PageModel
    {
        private readonly IUserService _userService;
        public new User? User { get; set; }
        public PlaylistResponse? Playlists { get; set; }
        public ProfileModel(IUserService userService)
        {
            _userService = userService;
        }
        public async Task OnGetAsync()
        {
            Playlists = new PlaylistResponse();
            User = new();
            User = await _userService.GetUser();
            Playlists = await _userService.GetPlaylists();
        }
    }
}
