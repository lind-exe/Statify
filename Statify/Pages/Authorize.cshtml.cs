using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Statify.Interfaces;
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

        private const string ClientId = "de733e5c3f6b418a97c787f8abe82ba5";
        private const string RedirectUri = "https://localhost:7274";
        private const string Scope = "user-read-private user-read-email";

        public IActionResult OnGet()
        {
            return Page();
        }

        public IActionResult OnPostAuthorize()
        {
            var codeChallenge = _authorizationService.GenerateCodeChallenge();
            HttpContext.Session.SetString("code_verifier", codeChallenge);

            string authUrl = $"https://accounts.spotify.com/authorize";
            var queryParams = new StringBuilder();
            queryParams.Append($"?response_type=code");
            queryParams.Append($"&client_id={ClientId}");
            queryParams.Append($"&scope={Scope}");
            queryParams.Append($"&code_challenge_method=S256");
            queryParams.Append($"&code_challenge={codeChallenge}");
            queryParams.Append($"&redirect_uri={RedirectUri}");

            return Redirect(authUrl + queryParams.ToString());
        }
    }
}
