using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Statify.Interfaces;
using Statify.Models;

namespace Statify.Pages
{
    public class ProfileModel : PageModel
    {
        private IUserService _userService;
        [BindProperty]
        public new User? User { get; set; }

        public ProfileModel(IUserService userService)
        {
            _userService = userService;
        }

        public async Task OnGetAsync()
        {
            User = new();
            User = await _userService.GetUserFromSpotifyWithWebApi();

        }
    }
}