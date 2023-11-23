using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Statify.Interfaces;
using Statify.Models;
using System.Text;

namespace Statify.Pages
{
    public class AuthorizeModel : PageModel
    {
        private readonly IAuthorizationService _authorizationService;

        public AuthorizeModel(IAuthorizationService authorizationService)
        {
            _authorizationService = authorizationService;
        }

        private const string RedirectUri = "https://localhost:7274";
        private const string Scope = "user-read-private user-read-email";

        public IActionResult OnGet()
        {
            return Page();
        }

        public IActionResult OnPostAuthorize()
        {
            _authorizationService.GenerateCodeChallenge();
            string authUrl = $"https://accounts.spotify.com/authorize";
            var queryParams = new StringBuilder();
            queryParams.Append($"?response_type=code");
            queryParams.Append($"&client_id={SpotifyAPICodes.ClientId}");
            queryParams.Append($"&scope={Scope}");
            queryParams.Append($"&code_challenge_method=S256");
            queryParams.Append($"&code_challenge={SpotifyAPICodes.CodeChallenge}");
            queryParams.Append($"&redirect_uri={RedirectUri}");

            return Redirect(authUrl + queryParams.ToString());
        }
    }
}
