using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using System.Net.Http;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Statify.Models;
using System.Text.Json;
using System.Net;
using System.Text;
using Statify.Interfaces;

namespace Statify.Pages
{
    public class IndexModel : PageModel
    {
        public PkceAuthorization? Authentication { get; set; }
        private readonly ISpotifyService _spotifyService;

        public IndexModel(ISpotifyService spotifyService)
        {
            _spotifyService = spotifyService;
        }
        public async Task<IActionResult> OnGetAsync(string code)
        {
            Authentication = HttpContext.Session.GetObjectFromJson<PkceAuthorization>("User");
            Authentication ??= new PkceAuthorization { Authenticated = false };

            if (!Authentication.Authenticated && string.IsNullOrEmpty(code))
            {
                var redirectUrl = _spotifyService.AuthorizeUser();
                return Redirect(redirectUrl);
            }

            if (!Authentication.Authenticated && !string.IsNullOrEmpty(code))
            {
                await _spotifyService.ExchangeCodeForTokenAsync(code);
                
            }
            return RedirectToPage("/Profile");
        }
    }
}
