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
        private const string RedirectUri = "https://localhost:7274";
        private const string Scope = "user-read-private user-read-email";
        public PkceAuthorization? Authentication { get; set; }

        private readonly IAuthorizationService _authorizationService;

        public IndexModel(IAuthorizationService authorizationService)
        {
            _authorizationService = authorizationService;
        }

        public IActionResult Init()
        {
            _authorizationService.GenerateCodeChallenge();
            string authUrl = $"https://accounts.spotify.com/authorize";
            var queryParams = new StringBuilder();
            queryParams.Append($"?response_type=code");
            queryParams.Append($"&client_id={SpotifyApiCodes.ClientId}");
            queryParams.Append($"&scope={Scope}");
            queryParams.Append($"&code_challenge_method=S256");
            queryParams.Append($"&code_challenge={SpotifyApiCodes.CodeChallenge}");
            queryParams.Append($"&redirect_uri={RedirectUri}");

            return Redirect(authUrl + queryParams.ToString());
        }

        public async Task<IActionResult> OnGetAsync(string code)
        {
            Authentication = HttpContext.Session.GetObjectFromJson<PkceAuthorization>("User");
            Authentication ??= new PkceAuthorization { Authenticated = false };

            if (!Authentication.Authenticated && string.IsNullOrEmpty(code))
            {
                return Init();
            }

            if (!Authentication.Authenticated && !string.IsNullOrEmpty(code))
            {
                await ExchangeCodeForTokenAsync(code);
                
            }
            return RedirectToPage("/Profile");
        }

        private async Task<IActionResult> ExchangeCodeForTokenAsync(string code)
        {
            Authentication = HttpContext.Session.GetObjectFromJson<PkceAuthorization>("User");

            using var httpClient = new HttpClient();
            var tokenRequest = new Dictionary<string, string>
            {
                { "client_id", SpotifyApiCodes.ClientId },
                { "grant_type", "authorization_code" },
                { "code", code },
                { "redirect_uri", RedirectUri },
                { "code_verifier", SpotifyApiCodes.CodeVerifier! },
            };

            var content = new FormUrlEncodedContent(tokenRequest);

            var response = await httpClient.PostAsync("https://accounts.spotify.com/api/token", content);

            if (response.IsSuccessStatusCode)
            {
                string responseString = await response.Content.ReadAsStringAsync();
                Authentication = JsonSerializer.Deserialize<PkceAuthorization>(responseString);

                if (Authentication is not null)
                {
                    Authentication.Authenticated = true;
                    HttpContext.Session.SetObjectAsJson("User", Authentication);
                }
            }
            else
            {
                // Handle the error, log the response, etc.

            }
            return Page();
        }
    }
}
