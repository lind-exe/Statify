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
        public PKCEAuthorization Authentication { get; set; }

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
            queryParams.Append($"&client_id={SpotifyAPICodes.ClientId}");
            queryParams.Append($"&scope={Scope}");
            queryParams.Append($"&code_challenge_method=S256");
            queryParams.Append($"&code_challenge={SpotifyAPICodes.CodeChallenge}");
            queryParams.Append($"&redirect_uri={RedirectUri}");

            return Redirect(authUrl + queryParams.ToString());
        }

        public async Task<IActionResult> OnGetAsync(string code)
        {
            // Retrieve user information from session
            Authentication = HttpContext.Session.GetObjectFromJson<PKCEAuthorization>("User");
            Authentication ??= new PKCEAuthorization { Authenticated = false };

            // If the user is not authenticated and there is no authorization code
            if (!Authentication.Authenticated && string.IsNullOrEmpty(code))
            {
                // Initiate the authorization flow by redirecting to Spotify
                return Init();
            }

            // If an authorization code is present and the user is not authenticated
            if (!Authentication.Authenticated && !string.IsNullOrEmpty(code))
            {
                Console.WriteLine($"Received authorization code: {code}");
                var tokenResponse = await ExchangeCodeForTokenAsync(code);
                // Process token response as needed
            }

            // Return the page
            return Page();
        }

        private async Task<IActionResult> ExchangeCodeForTokenAsync(string code)
        {
            Authentication = HttpContext.Session.GetObjectFromJson<PKCEAuthorization>("User");

            using var httpClient = new HttpClient();
            var tokenRequest = new Dictionary<string, string>
            {
                { "client_id", SpotifyAPICodes.ClientId },
                { "grant_type", "authorization_code" },
                { "code", code },
                { "redirect_uri", RedirectUri },
                { "code_verifier", SpotifyAPICodes.CodeVerifier },
            };

            var content = new FormUrlEncodedContent(tokenRequest);

            var response = await httpClient.PostAsync("https://accounts.spotify.com/api/token", content);

            if (response.IsSuccessStatusCode)
            {
                string responseString = await response.Content.ReadAsStringAsync();
                Authentication = JsonSerializer.Deserialize<PKCEAuthorization>(responseString);
                Authentication.Authenticated = true;
                HttpContext.Session.SetObjectAsJson("User", Authentication);
            }
            else
            {
                // Handle the error, log the response, etc.

            }
            return Page();
        }
    }
}
